using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CORiverDesignations
{
    public abstract class BaseConveyance : IDisposable
    {
        public BaseConveyance()
        {


        }
        public abstract void Initialize();
       // public abstract void Allottments();
        public abstract void Allottments(int year, double convey);
        public abstract void Allottments(int year, int elevation, int state, out double convey);
        public abstract void Allocate(int year, double convey);
        public abstract void Allocate(int year, int elevation, int state, double convey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //
            if (disposing)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    public abstract class BaseAllottments : IDisposable
    {
        internal int Felevation;
        internal double FLivePool;
        internal double FThreshold;
        //
        //internal double FTrigger_1;
        //internal double FTrigger_2;
        //internal double FTrigger_3;
        //internal double FTrigger_4;
        //
        public BaseAllottments() { }
        public BaseAllottments(int elevation) {
            Felevation = elevation;
        }
        public BaseAllottments(double volumePool) {
            FLivePool = volumePool;

        }
        //
        public BaseAllottments(double volumePool, double threshold)
        {
            FLivePool = volumePool;
            FThreshold = threshold; 
        }


        // =========================================
        // properties;
        double _initial = 0;
        public double InitialRight
        {
            set { _initial = value; }
            get { return _initial; }
        }
        //
        double _legalRightOne = 0;
        public double LegalRightOne
        {
            set { _legalRightOne = value; }
            get { return _legalRightOne; }
        }
        //
        double _legalRightTwo = 0;
        public double LegalRightTwo
        {
            set { _legalRightTwo = value; }
            get { return _legalRightTwo; }
        }
        //
        double _dependent = 0;
        public double DependentRight
        {
            set { _dependent = value; }
            get { return _dependent; }
        }
        //
        double _threshold = 0;
        public double Threshold
        {
            set { _threshold = value; }
            get { return _threshold; }
        }
        double _targetOne = 0;
        public double TargetOne
        {
            set { _targetOne = value; }
            get { return _targetOne; }
        }
        //
        double _reduceLegalRightOne = 1;
        public double ReduceLegalRightThreeOne
        {
            set { _reduceLegalRightOne = value; }
            get { return _reduceLegalRightOne; }
        }
        double _reduceLegalRightTwo = 1;
        public double ReduceLegalRightThreeTwo
        {
            set { _reduceLegalRightTwo = value; }
            get { return _reduceLegalRightTwo; }
        }

        private double[] _RightHolder = new double[35];
        public double [] LegalRights
        {
            get { return _RightHolder; }
            set { _RightHolder = value; }
        }
        private double[] _ObservedRights = new double[35];
        public double[] ObservedRights
        {
            get { return _ObservedRights; }
            set { _ObservedRights = value; }
        }
        // ==================================
         public enum RightHoldersP1
        {
            one,
            two,
            three,
            four,
            Max
        }
        public enum RightHoldersP2
        {
            one,
            two,
            three,
            four,
            Max
        }
        public enum RightHoldersP3
        {
            AkChin,
            SevenCities,
            three,
            four,
            Max
        }
        public float[] Priority_P1 = new float[(int)RightHoldersP1.Max];
        public float[] Priority_P2 = new float[(int)RightHoldersP2.Max] ;
        public float[] Priority_P3 = new float[(int)RightHoldersP1.Max];
        public float[] Priority_P4 = new float[(int)RightHoldersP1.Max];
        //
        public float[] RealizedRight_P1 = new float[(int)RightHoldersP1.Max];
        public float[] RealizedRight_P2 = new float[(int)RightHoldersP1.Max];
        public float[] RealizedRight_P3 = new float[(int)RightHoldersP1.Max];
        public float[] RealizedRight_P4 = new float[(int)RightHoldersP1.Max];
        // ==========================================
        // Methods;
        public List<string> data = new List<string>();
       internal  List<string> ReadIt(string fileName)
        {
            string line;
            StreamReader file = new StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            { if (line != "") data.Add(line); }
            file.Close();
            return data;
        }


        // ==========================================
        public abstract int Elevation();
        public abstract double LivePoolVolume();
        public abstract bool VolumeAllottments(int priority, double volume);
        public abstract void Allottments(int year, double volume);
        public abstract void Allottments(int year, int elevation, double volume);
        public abstract void Allottments(int year, int elevation, int state,double volume);
        //
        public abstract void Initialize();
        //







        protected virtual void Dispose(bool disposing)
        {
            //
            if (disposing)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
