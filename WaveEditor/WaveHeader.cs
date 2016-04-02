using System;
using System.Runtime.InteropServices;
using static System.Text.Encoding;

// Grayden Hormes
// A00925756

namespace WaveEditor {
    public class WaveHeader {

        // RIFF chunk

        /// <summary>
        /// Contains the letters "RIFF" in ASCII form
        /// </summary>
        public string RIFFChunkID;

        /// <summary>
        /// <para/>36 + SubChunk2Size, or more precisely: 
        /// <para/>4 + (8 + SubChunk1Size) + (8 + SubChunk2Size)
        /// This is 
        /// the size of the rest of the chunk
        /// following this number. <para/> This is the size of the
        /// entire file in bytes minus 8 bytes for the
        /// two fields not included in this count:
        /// <para/>ChunkID and ChunkSize.
        /// </summary>
        public uint RIFFChuckSize;

        /// <summary>
        /// Contains the letters "WAVE"
        /// </summary>
        public string format;

        // fmt chunk

        /// <summary>
        /// Contains the letters "fmt "
        /// </summary>
        public string fmtChuckID;

        /// <summary>
        /// 16 for PCM.  This is the size of the
        /// rest of the Subchunk which follows this number.
        /// </summary>
        public uint fmtChunkSize;

        /// <summary>
        /// PCM = 1 (i.e. Linear quantization)
        /// Values other than 1 indicate some
        /// form of compression.
        /// </summary>
        public uint audioFormat;

        /// <summary>
        /// Mono = 1, Stereo = 2, etc.
        /// </summary>
        public uint numChannels;

        /// <summary>
        /// 8000, 44100, etc.
        /// </summary>
        public uint sampleRate;

        /// <summary>
        /// == SampleRate * NumChannels * BitsPerSample/8
        /// </summary>
        public uint byteRate;

        /// <summary>
        /// == NumChannels * BitsPerSample/8 <para/>
        /// The number of bytes for one sample including
        /// all channels.
        /// </summary>
        public uint blockAlign;

        /// <summary>
        /// 8 bits = 8, 16 bits = 16, etc.
        /// </summary>
        public uint bitsPerSample;

        // data chunk

        /// <summary>
        /// Contains the letters "data"
        /// </summary>
        public string dataChunkID;

        /// <summary>
        ///  == NumSamples * NumChannels * BitsPerSample/8
        /// <para/>This is the number of bytes in the data.
        /// <para/>You can also think of this as the size
        /// of the read of the subchunk following this
        /// number.
        /// </summary>
        public uint dataChunkSize;


        public WaveHeader(string rci, uint rcs, string fmt, // RIFF
                          string fci, uint fcs, uint af, uint nc, uint sr, uint br, uint ba, uint bps, // fmt
                          string dci, uint dcs /* data */) {
            // RIFF chunk
            RIFFChunkID = rci;
            RIFFChuckSize = rcs;
            format = fmt;

            // fmt chuck
            fmtChuckID = fci;
            fmtChunkSize = fcs;
            audioFormat = af;
            numChannels = nc;
            sampleRate = sr;
            byteRate = br;
            blockAlign = ba;
            bitsPerSample = bps;

            // data chunk
            dataChunkID = dci;
            dataChunkSize = dcs;
        }

      /*public byte[] GetValues() {
            byte[] result = new byte[44];

            // RIFF chunk
            Buffer.BlockCopy("RIFF".ToCharArray(), 0, result, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(RIFFChuckSize), 0, result, 4, 4);
            Buffer.BlockCopy("wave".ToCharArray(), 0, result, 8, 4);

            // fmt chuck
            Buffer.BlockCopy("fmt".ToCharArray(), 0, result, 12, 3);
            Buffer.BlockCopy(BitConverter.GetBytes(fmtChunkSize), 0, result, 16, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(audioFormat), 0, result, 20, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(numChannels), 0, result, 22, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(sampleRate), 0, result, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(byteRate), 0, result, 28, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(blockAlign), 0, result, 32, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(bitsPerSample), 0, result, 34, 2);

            // data chunk
            Buffer.BlockCopy("data".ToCharArray(), 0, result, 36, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(dataChunkSize), 0, result, 40, 4);

            return result;
        }*/

        public WaveHeader(byte[] bytes) {
            byte[] temp;

            // RIFF chunk
            temp = ReadField(bytes[0], bytes[1], bytes[2], bytes[3]);
            RIFFChunkID = Default.GetString(temp);
            temp = ReadField(bytes[4], bytes[5], bytes[6], bytes[7]);
            RIFFChuckSize = BitConverter.ToUInt32(temp, 0);
            temp = ReadField(bytes[8], bytes[9], bytes[10], bytes[11]);
            format = Default.GetString(temp);

            // fmt chuck
            temp = ReadField(bytes[12], bytes[13], bytes[14], bytes[15]);
            fmtChuckID = Default.GetString(temp);
            temp = ReadField(bytes[16], bytes[17], bytes[18], bytes[19]);
            fmtChunkSize = BitConverter.ToUInt32(temp, 0);
            temp = ReadField(bytes[20], bytes[21]);
            audioFormat = BitConverter.ToUInt32(temp, 0);
            temp = ReadField(bytes[22], bytes[23]);
            numChannels = BitConverter.ToUInt32(temp, 0);
            temp = ReadField(bytes[24], bytes[25], bytes[26], bytes[27]);
            sampleRate = BitConverter.ToUInt32(temp, 0);
            temp = ReadField(bytes[28], bytes[29], bytes[30], bytes[31]);
            byteRate = BitConverter.ToUInt32(temp, 0);
            temp = ReadField(bytes[32], bytes[33]);
            blockAlign = BitConverter.ToUInt32(temp, 0);
            temp = ReadField(bytes[34], bytes[35]);
            bitsPerSample = BitConverter.ToUInt32(temp, 0);

            // data chunk
            temp = ReadField(bytes[36], bytes[37], bytes[38], bytes[39]);
            dataChunkID = Default.GetString(temp);
            temp = ReadField(bytes[40], bytes[41], bytes[42], bytes[43]);
            dataChunkSize = BitConverter.ToUInt32(temp, 0);
        }

        private byte[] ReadField(byte b1, byte b2, byte b3 = 0, byte b4 = 0) {
            byte[] result = new byte[4];
            
            result[0] = b1;
            result[1] = b2;
            result[2] = b3;
            result[3] = b4;

            return result;
        }
    }
}
