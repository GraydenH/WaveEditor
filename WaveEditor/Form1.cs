using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static WaveEditor.Sound;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;

// Grayden Hormes
// A00925756

namespace WaveEditor {
    public partial class Form1 : Form {

        int bitDepth = 16;                                           // desired bit depth for record and playback
        int sampleRate = 22050;                                      // desired sample rate for record and playback
        double[] samples, frequencies;                               // time and frequency domains
        int samplesStart = 0, samplesEnd = 0, samplesDiff = 0;       // selection variables in the time domain
        int filterStart = 0, filterEnd = 0, filterDiff = 0;          // selection variables in the frequency domain
        bool triangle = false, hamming = true;                       // windowing flags
        bool high = false, low = false, band = false;                // filter pass flags
        ToolStripMenuItem windowingRadio = new ToolStripMenuItem();  // tracks which item is selected on the windowing strip
        ToolStripMenuItem filteringRadio = new ToolStripMenuItem();  // tracks which item is selected on the filtering strip
        ToolStripMenuItem samplingRadio = new ToolStripMenuItem();   // tracks which item is selected on the sampling strip
        ToolStripMenuItem bitDepthRadio = new ToolStripMenuItem();   // tracks which item is selected on the bit depth strip
        Sound sound = new Sound();
        WaveHeader wv;

        public Form1() {
            InitializeComponent();
        }

        /// <summary>
        /// closes hidden win32 dialog when the form closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseForm(object sender, FormClosingEventArgs e) {
            Debug.WriteLine(CloseDialog() ? "close dialog succeeded" : "close dialog failed");
        }

        /// <summary>
        /// opens hidden win32 dialog and disables appropriate
        /// control buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadForm(object sender, EventArgs e) {
            Debug.WriteLine(OpenDialog() ? "open dialog succeeded" : "open dialog failed");
            playGroup.Controls[0].Enable(false);   // stop play
            playGroup.Controls[1].Enable(false);   // pause record
            recordGroup.Controls[0].Enable(false); // stop record
        }

        public Chart getChart() {
            return sampleChart;
        }

        /// <summary>
        /// Opens files and prints out the header.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open(object sender, EventArgs e) {
            sampleChart.Series["Samples"].Points.Clear();

            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == DialogResult.OK) {
                byte[] wave = File.ReadAllBytes(fileDialog.FileName);
                byte[] header = new List<byte>(wave).GetRange(0, 44).ToArray();
                wv = new WaveHeader(header);
                wave = new List<byte>(wave).GetRange(44, wave.Length - 44).ToArray();

                Debug.WriteLine("\nOpening File:"
                    + "\nChunk ID:        " + wv.RIFFChunkID
                    + "\nRIFF Chuck Size: " + wv.RIFFChuckSize
                    + "\nFormat:          " + wv.format
                    + "\nfmt Chuck ID:    " + wv.fmtChuckID
                    + "\nfmt Chunk Size:  " + wv.fmtChunkSize
                    + "\naudio format:    " + wv.audioFormat
                    + "\nNum Channels:    " + wv.numChannels
                    + "\nSample Rate:     " + wv.sampleRate
                    + "\nByte Rate:       " + wv.byteRate
                    + "\nBlock Align:     " + wv.blockAlign
                    + "\nBits Per Sample: " + wv.bitsPerSample
                    + "\nData Chunk ID:   " + wv.dataChunkID
                    + "\nData Chunk Size: " + wv.dataChunkSize);

                samples = new double[(int)(wv.dataChunkSize / wv.blockAlign)];
                short[] temp = new short[samples.Length];

                for (int i = 0, j = 0; i < wv.dataChunkSize-4; i += (int)wv.blockAlign, j++)
                    temp[j] = BitConverter.ToInt16(wave, i);
                samples = temp.Select(x => (double)(x)).ToArray();
                
                PlotSamples(samples);
            }
        }

        /// <summary>
        /// plots any double[] to the top chart/time domain.
        /// </summary>
        /// <param name="samples">samples to be plotted
        /// on the time domain</param>
        public void PlotSamples(double[] samples) {
            sampleChart.Series["Samples"].Points.Clear();
            Series series = sampleChart.Series["Samples"];
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Double;

            if (samples != null)
                for (int i = 0; i < samples.Length; i++) 
                    sampleChart.Series["Samples"].Points.AddXY(i, samples[i]);

            ChartArea chartArea = sampleChart.ChartAreas[sampleChart.Series["Samples"].ChartArea];
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = samples.Length;
            chartArea.AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chartArea.AxisX.ScrollBar.ButtonColor = Color.White;
        }

        /// <summary>
        /// event for DFT button click, starts a background worker.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartDFT(object sender, EventArgs e) {
            if (samples == null)
                return;
            if (DFTWorker.IsBusy != true) {
                foreach (Control c in Controls) c.Enable(false);
                DFTWorker.RunWorkerAsync();
            }
        }
        
        /// <summary>
        /// bulk ok the work done by the background worker,
        /// starts a number of tasks on doing a DFT over the
        /// samples then waits for all the tasks to finish,
        /// but it in background preventing the message
        /// loop from being blocked and the UI thread from
        /// freezing up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DFTWork(object sender, DoWorkEventArgs e) {
            double[] temp = new List<double>(samples)
                .GetRange(samplesStart, samplesDiff).ToArray();

            if (triangle)
                temp = TriangleWindow(temp);
            else if (hamming)
                temp = HammingWindow(temp);

            sound.SetSamples(temp);
            Task[] tasks = new Task[8]; // same as threads

            for (int i = 0; i < tasks.Length; i++) {
                tasks[i] = new Task(() => sound.DFT());
                tasks[i].Start();
            }

            // worker is not done until all threads are done
            Task.WaitAll(tasks);
        }
        
        /// <summary>
        /// plots the completed DFT once the worker is finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DFTCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null)
                Debug.WriteLine("Error: " + e.Error.Message);
            else {
                frequencies = sound.GetFreq();
                Series barSeries = fequencyChart.Series["Frequency Bins"];
                barSeries.Points.DataBindY(frequencies);
                foreach (Control c in Controls) c.Enable(true);
            }
        }

        /// <summary>
        /// keeps track of the latest change to 
        /// which samples have been selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SamplesSelected(object sender, CursorEventArgs e) {
            if (samples == null)
                return;
            samplesStart = (int)e.NewSelectionStart;
            samplesEnd = (int)e.NewSelectionEnd;
            samplesDiff = samplesEnd - samplesStart;

            if (samplesDiff < 0) {
                int temp = samplesEnd;
                samplesEnd = samplesStart;
                samplesStart = temp;
                samplesDiff = samplesEnd - samplesStart;
            }
        }
        
        /// <summary>
        /// keeps track of the latest frequency bins that
        /// have been selected. flips the selection to the 
        /// corresponding bin if they select past the
        /// nyquist limit (midway point of the bar chart).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterSelected(object sender, CursorEventArgs e) {
            if (frequencies == null)
                return;
            filterStart = (int)e.NewSelectionStart;
            filterEnd = (int)e.NewSelectionEnd;
            
            if (filterEnd > frequencies.Length / 2)
                filterEnd = frequencies.Length / 2;
            if (filterStart > frequencies.Length / 2)
                filterStart = filterStart - frequencies.Length / 2;

            if (filterDiff < 0) {
                int temp = filterEnd;
                filterEnd = filterStart;
                filterStart = temp;
            }
            filterDiff = filterEnd - filterStart;

            if (filterStart == 0)
                filterStart++;
            if (filterEnd < filterStart)
                filterEnd = filterStart;
        }

        /// <summary>
        /// saves files to the file system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, EventArgs e) {
            if (wv == null)
                return;
            short[] shorts = samples.Select(x => (short)(x)).ToArray();
            byte[] data = shorts.Select(x => Convert.ToInt16(x))
                .SelectMany(x => BitConverter.GetBytes(x)).ToArray();
            byte[] header = new byte[44];

            wv.numChannels = 1;
            wv.blockAlign = wv.blockAlign / wv.numChannels;

            Buffer.BlockCopy(wv.RIFFChunkID.ToCharArray(), 0, header, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.RIFFChuckSize), 0, header, 4, 4);
            Buffer.BlockCopy(wv.format.ToCharArray(), 0, header, 8, 4);
            Buffer.BlockCopy(wv.fmtChuckID.ToCharArray(), 0, header, 12, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.fmtChunkSize), 0, header, 16, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.audioFormat), 0, header, 20, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.numChannels), 0, header, 22, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.sampleRate), 0, header, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.byteRate), 0, header, 28, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.blockAlign), 0, header, 32, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.bitsPerSample), 0, header, 34, 2);
            Buffer.BlockCopy(wv.dataChunkID.ToCharArray(), 0, header, 36, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(wv.dataChunkSize), 0, header, 40, 4);

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "WAV|*.wav";
            if (saveDialog.ShowDialog() == DialogResult.OK) {
                using (Stream s = File.Open(saveDialog.FileName, FileMode.CreateNew)) {
                    using (BinaryWriter bw = new BinaryWriter(s)) {
                        bw.Write(header);
                        bw.Write(data);
                    }
                }
            }
        }

        /// <summary>
        /// copies and removes a section of samples.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cut(object sender, EventArgs e) {
            if (samples == null)
                return;

            double[] start = new List<double>(samples)
                .GetRange(0, samplesStart).ToArray();
            double[] end = new List<double>(samples)
                .GetRange(samplesEnd, samples.Length - samplesEnd).ToArray();
            double[] clipboard = new List<double>(samples)
                .GetRange(samplesStart, samplesDiff).ToArray();

            string str = "";
            foreach (double d in clipboard)
                str += d.ToString() + "|";
            
            Clipboard.SetData(DataFormats.Text, str);

            samples = new double[samples.Length - (samplesDiff)];
            start.CopyTo(samples, 0);
            end.CopyTo(samples, start.Length);

            wv.RIFFChuckSize -= (uint)samplesDiff;
            wv.dataChunkSize -= (uint)samplesDiff;
            PlotSamples(samples);
        }

        /// <summary>
        /// pastes the last copied section of samples.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Paste(object sender, EventArgs e) {
            string str = (string)Clipboard.GetData(DataFormats.Text);
            if (str == null)
                return;

            string[] vals = str.Split('|');
            double[] clipboard = new double[vals.Length];
            for (int i = 0; i < vals.Length - 1; i++)
                clipboard[i] = Convert.ToDouble(vals[i]);

            double[] start = new List<double>(samples)
                .GetRange(0, samplesStart).ToArray();
            double[] end = new List<double>(samples)
                .GetRange(samplesEnd, samples.Length - samplesEnd).ToArray();
            samples = new double[samples.Length + clipboard.Length];

            start.CopyTo(samples, 0);
            clipboard.CopyTo(samples, start.Length);
            end.CopyTo(samples, start.Length + clipboard.Length);

            wv.RIFFChuckSize += (uint)samplesDiff;
            wv.dataChunkSize += (uint)samplesDiff;
            PlotSamples(samples);
        }

        /// <summary>
        /// copies a selected section of samples.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Copy(object sender, EventArgs e) {
            if (samples == null)
                return;

            double[] clipboard = new List<double>(samples)
                .GetRange(samplesStart, samplesDiff).ToArray();

            string str = "";
            foreach (double d in clipboard)
                str += d.ToString() + "|";
            
            Clipboard.SetData(DataFormats.Text, str);
        }

        /// <summary>
        /// sets the windowing function. must be done unless 
        /// a square window is desired.
        /// <para/>triangle or hamming
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetWindow(object sender, EventArgs e) {
            ToolStripMenuItem selected = (ToolStripMenuItem)sender;

            if (selected.Name == "hammingItem") {
                hamming = true;
                triangle = false;
            } else if (selected.Name == "triangleItem") {
                triangle = true;
                hamming = false;
            }

            windowingRadio.CheckState = CheckState.Unchecked;
            windowingRadio = selected;
            windowingRadio.CheckState = CheckState.Checked;
        }

        /// <summary>
        /// sets the bit depth for recording.
        /// <para/>16bit or 32bit. 16bit default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBitDepth(object sender, EventArgs e) {
            ToolStripMenuItem selected = (ToolStripMenuItem)sender;

            if (selected.Name == "SixteenBitItem") {
                bitDepth = 16;
            } else if (selected.Name == "ThirtyTwoBitItem") {
                bitDepth = 32;
            }

            bitDepthRadio.CheckState = CheckState.Unchecked;
            bitDepthRadio = selected;
            bitDepthRadio.CheckState = CheckState.Checked;
        }

        /// <summary>
        /// sets the sampling rate for recording.
        /// <para/>11025Hz, 22050Hz, or 44100Hz. 22050Hz default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSamplingRate(object sender, EventArgs e) {
            ToolStripMenuItem selected = (ToolStripMenuItem)sender;

            if (selected.Name == "rate1Item") {
                sampleRate = 11025;
            } else if (selected.Name == "rate2Item") {
                sampleRate = 22050;
            } else if (selected.Name == "rate3Item") {
                sampleRate = 44100;
            }

            samplingRadio.CheckState = CheckState.Unchecked;
            samplingRadio = selected;
            samplingRadio.CheckState = CheckState.Checked;
        }

        /// <summary>
        /// sets the filter behaviour. Must select one to do a filter.
        /// <para/>high pass, lowp pass, or band pass. no default.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetFilter(object sender, EventArgs e) {
            ToolStripMenuItem selected = (ToolStripMenuItem)sender;

            if (selected.Name == "bandPassItem") {
                band = true;
                high = false;
                low = false;
            } else if (selected.Name == "lowPassItem") {
                low = true;
                high = false;
                band = false;
            } else if (selected.Name == "highPassItem") {
                high = true;
                low = false;
                band = false;
            }

            filteringRadio.CheckState = CheckState.Unchecked;
            filteringRadio = selected;
            filteringRadio.CheckState = CheckState.Checked;
        }

        /// <summary>
        /// zooms in or out to the selected range.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Magnify(object sender, EventArgs e) {
            sampleChart.ChartAreas[0].AxisX.ScaleView.Zoom(samplesStart, samplesEnd);
        }

        /// <summary>
        /// filters over the designed filter using
        /// the selected filter type.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter(object sender, EventArgs e) {
            if (samples == null || frequencies == null)
                return;
            double[] weights = new double[0];

            if (high)
                weights = HighPassFilter();
            else if (low)
                weights = LowPassFiler();
            else if (band) {
                weights = BandPassFilter();
                if (weights == null) return;
            } else return;

            double[] old = samples;
            samples = new double[old.Length + (old.Length % weights.Length)];
            wv.RIFFChuckSize += (uint)(old.Length % weights.Length);
            wv.dataChunkSize += (uint)(old.Length % weights.Length);

            for (int j = 0; j < samples.Length; j++)
                if (j < old.Length)
                    samples[j] = old[j];
                else
                    samples[j] = 0; // pad with 0's

            double val;
            for (int n = 0; n < (samples.Length - weights.Length); n++) {
                val = 0;
                for (int m = 0; m < weights.Length; m++)
                    val += samples[n + m] * weights[m];
                samples[n] = val;
            }

            Series barSeries = fequencyChart.Series["Frequency Bins"];
            barSeries.Points.DataBindY(frequencies);
            PlotSamples(samples);
        }

        /// <summary>
        /// creates the mask for a band pass filter
        /// and returns the IDFT weights for that mask
        /// </summary>
        /// <returns>IDFT weights</returns>
        private double[] BandPassFilter() {
            if (filterDiff == 0)
                return null;

            int[] mask = new int[frequencies.Length - 1];
            for (int x = 0; x < mask.Length; x++)
                mask[x] = 1;
            for (int i = 1; i < frequencies.Length; i++) {
                if (i == filterStart || i == (frequencies.Length + filterEnd))
                    i += filterDiff;
                mask[i - 1] = 0;
            }

            return IDFT(mask);
        }

        /// <summary>
        /// creates the mask for a low pass filter
        /// and returns the IDFT weights for that mask
        /// </summary>
        /// <returns>IDFT weights</returns>
        private double[] LowPassFiler() {
            if (filterEnd == 0) {
                filterEnd++;
                filterDiff++;
            }

            int[] mask = new int[frequencies.Length - 1];
            for (int x = 0; x < mask.Length; x++)
                mask[x] = 1;
            for (int i = filterEnd; i < (frequencies.Length - filterEnd); i++)
                mask[i] = 0;

            return IDFT(mask);
        }

        /// <summary>
        /// creates the mask for a high pass filter
        /// and returns the IDFT weights for that mask
        /// </summary>
        /// <returns>IDFT weights</returns>
        private double[] HighPassFilter() {
            int[] mask = new int[frequencies.Length - 1];
            for (int i = 1; i < frequencies.Length - 1; i++) 
                if (i <= filterStart || i >= (frequencies.Length - filterStart))
                    mask[i - 1] = 0;
                else
                    mask[i - 1] = 1;

            return IDFT(mask);
        }

        /// <summary>
        /// takes in recorded data, re enables most of the UI.
        /// gathers a wave form form the recording and puts
        /// it into the wave header wv. then prints this
        /// information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StopRecord(object sender, EventArgs e) {
            RecordData rd = StopRec();
            byte[] data = new byte[rd.len];
            Marshal.Copy(rd.ip, data, 0, (int)rd.len);

            WaveForm wf = (WaveForm)Marshal.PtrToStructure(GetWaveform(), typeof(WaveForm));
            wv = new WaveHeader("RIFF", rd.len + 36, "WAVE",
                "fmt", 16, 1, wf.nChannels, wf.nSamplesPerSec,
                wf.nAvgBytesPerSec, wf.nBlockAlign, wf.wBitsPerSample,
                "data", rd.len);

            if (wv.bitsPerSample == 32) {
                int[] temp = new int[rd.len / (int)wv.blockAlign];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = BitConverter.ToInt32(data, i * (int)wv.blockAlign);
                samples = temp.Select(x => (double)(x)).ToArray();
            } else if (wv.bitsPerSample == 16) {
                short[] temp = new short[rd.len / (int)wv.blockAlign];
                for (int i = 0; i < temp.Length-1; i++)
                    temp[i] = BitConverter.ToInt16(data, i * (int)wv.blockAlign);
                samples = temp.Select(x => (double)(x)).ToArray();
            } 

            Debug.WriteLine("\nRecorded Wave::"
                + "\nChunk ID:        " + wv.RIFFChunkID
                + "\nRIFF Chuck Size: " + wv.RIFFChuckSize
                + "\nFormat:          " + wv.format
                + "\nfmt Chuck ID:    " + wv.fmtChuckID
                + "\nfmt Chunk Size:  " + wv.fmtChunkSize
                + "\naudio format:    " + wv.audioFormat
                + "\nNum Channels:    " + wv.numChannels
                + "\nSample Rate:     " + wv.sampleRate
                + "\nByte Rate:       " + wv.byteRate
                + "\nBlock Align:     " + wv.blockAlign
                + "\nBits Per Sample: " + wv.bitsPerSample
                + "\nData Chunk ID:   " + wv.dataChunkID
                + "\nData Chunk Size: " + wv.dataChunkSize);

            PlotSamples(samples);
            foreach (Control c in Controls) c.Enable(true);
            recordGroup.Controls[0].Enable(false);
        }

        /// <summary>
        /// starts the recording at the selected bit depth
        /// and sample rate. locks most of the UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StartRecord(object sender, EventArgs e) {
            foreach (Control c in Controls) if (c != recordGroup) c.Enable(false);
            recordGroup.Controls[0].Enable(true);
            Debug.WriteLine(StartRec(bitDepth, sampleRate) ? "start rec succeeded" : "start rec failed");
        }

        /// <summary>
        /// coverts samples to bytes of the selected 
        /// bit depth then sends the message to play.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartPlay(object sender, EventArgs e) {
            if (wv == null)
                return;
            foreach (Control c in Controls)
                if (c != playGroup)
                    c.Enable(false);
                else
                    c.Enable(true);

            byte[] data = null;
            if (wv.bitsPerSample == 16) {
                data = samples.Select(x => (short)(x)).ToArray()
                    .Select(x => Convert.ToInt16(x))
                    .SelectMany(x => BitConverter.GetBytes(x)).ToArray();
            } else if (wv.bitsPerSample == 32) {
                data = samples.Select(x => (int)(x)).ToArray()
                    .Select(x => Convert.ToInt32(x))
                    .SelectMany(x => BitConverter.GetBytes(x)).ToArray();
            }

            IntPtr iptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)) * data.Length);
            Marshal.Copy(data, 0, iptr, data.Length);
            Debug.WriteLine(PlayStart(iptr, data.Length, (int)wv.bitsPerSample, (int)wv.sampleRate) ? 
                "play start succeeded" : "play start failed");
            Marshal.FreeHGlobal(iptr);
        }

        /// <summary>
        /// sends the pause play message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PausePlay(object sender, EventArgs e) {
            Debug.WriteLine(PlayPause() ? "pause succeeded" : "pause failed");
        }

        /// <summary>
        /// sends the stop play message. unlocks
        /// the rest of the UI except for some elements
        /// which become locked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopPlay(object sender, EventArgs e) {
            Debug.WriteLine(PlayStop() ? "stop play succeeded" : "stop play failed");
            foreach (Control c in Controls) {
                if (c == playGroup) {
                    c.Controls[0].Enable(false);
                    c.Controls[1].Enable(false);
                    c.Controls[2].Enable(true);
                } else c.Enable(true);
            }
        }

        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool OpenDialog();
        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CloseDialog();
        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern RecordData StopRec();
        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool StartRec(int d, int r);
        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetWaveform();
        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool PlayPause();
        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool PlayStart(IntPtr p, int size, int d, int r);
        [DllImport("Record.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool PlayStop();

        /// <summary>
        /// equivalent to WAVEFORMATEX in the win32 header windows.h
        /// </summary>
        public struct WaveForm {
            public ushort wFormatTag;
            public ushort nChannels;
            public uint nSamplesPerSec;
            public uint nAvgBytesPerSec;
            public ushort nBlockAlign;
            public ushort wBitsPerSample;
            public ushort cbSize;
        }

        /// <summary>
        /// struct holding a pointer to the recorded data from the win32 DLL
        /// along with its length (pointers don't come with lengths)
        /// </summary>
        public struct RecordData {
            public uint len;
            public IntPtr ip;
        }
    }

    /// <summary>
    /// static class responsible for enabling/disabling
    /// a desired control. also recursively does the the
    /// same to all subcontrols.
    /// </summary>
    public static class GuiController {
        public static void Enable(this Control con, bool enable) {
            if (con != null) {
                foreach (Control c in con.Controls) c.Enable(enable);
                try { con.Invoke((MethodInvoker)(() => con.Enabled = enable)); } catch {}
            }
        }
    }
}
