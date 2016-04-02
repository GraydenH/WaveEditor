using System;
using System.Diagnostics;
using static System.Math;

// Grayden Hormes
// A00925756

namespace WaveEditor {
    class Sound {
        private static double[] A, s;
        private static int masterInd = 0;
        private object threadLock = new object();

        ///<summary>
        /// discrete fourier 
        /// <para/>Af = sum((st*cos(2pi * t*f/N) + st*sin(2pi * t*f/N)))
        ///</summary> 
        /// <param name="s"></param>
        /// <returns></returns>
        public void DFT() {
            double N = s.Length, real, imag;
            int f = masterInd;

            while (f <= N - 1) {
                lock (threadLock)
                    if (masterInd <= N - 1)
                        f = masterInd++;
                    else break;
                real = 0;
                imag = 0;
                for (int t = 0; t <= N - 1; t++) {
                    real += s[t] * Cos(2 * PI * t * f / N);
                    imag -= s[t] * Sin(2 * PI * t * f / N);
                }
                A[f] = Sqrt(real * real + imag * imag);
            }
        }

        ///<summary>
        /// inverse fourier
        /// <para/>st = sum((Af*cos(2pi * t*f/N) + Af*sin(2pi * t*f/N)))
        ///</summary> 
        /// <param name="mask">the filter mask</param>
        /// <returns></returns>
        public static double[] IDFT(int[] mask) {
            double real, imag, N = mask.Length;
            double[] result = new double[(int)N];

            for (int t = 0; t < N; t++) {
                real = 0;

                imag = 0;   
                for (int f = 0; f < N; f++) {
                    real += mask[f] * Cos(2 * PI * t * f / N);
                    imag += mask[f] * Sin(2 * PI * t * f / N);
                }
                result[t] = (real - imag) / N;
            }

            return result;
        }

        ///<summary>
        /// triangle windowing function
        /// <para/>(2 / N) * (N / 2 - Abs(n - (N - 1) / 2))
        ///</summary> 
        /// <param name="w">the unweighted values</param>
        /// <returns name="result">weighted values</returns>
        public static double[] TriangleWindow(double[] w) {
            double N = w.Length, weight;
            double[] result = new double[(int)N];

            for (int n = 0; n < N; n++) {
                weight = (2 / N) * (N / 2 - Abs(n - (N - 1) / 2));
                result[n] = weight * w[n];
            }

            return result;
        }

        ///<summary>
        /// hamming windowing function
        /// <para/>0.53836 - 0.46164 * Cos(2 * PI * n / (N - 1))
        ///</summary> 
        /// <param name="w">unweighted values</param>
        /// <returns name="result">weighted values</returns>
        public static double[] HammingWindow(double[] w) {
            double N = w.Length, weight;
            double[] result = new double[(int)N];

            for (int n = 0; n < N; n++) {
                weight = 0.53836 - 0.46164 * Cos(2 * PI * n / (N - 1));
                result[n] = weight * w[n];
            }

            return result;
        }

        ///<summary>
        /// get frequency domain values
        ///</summary> 
        /// <returns name="A">array of all the values for the frequency domain</returns>
        internal double[] GetFreq() {
            return A;
        }

        ///<summary>
        /// Set samples
        ///</summary> 
        /// <param name="samples">samples to be added to this instance of Sound</param>
        internal void SetSamples(double[] samples) {
            s = samples;
            A = new double[samples.Length];
            masterInd = 0;
        }
    }
}
