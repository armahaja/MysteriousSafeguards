using System;
using System.Collections.Generic;
using System.IO;

namespace Practice
{
    class Program
    {
        // this function read the input file and add each line to a arraylist
        public static List<string> ReadFromFile(string fileName)
        {
            List<string> fileData = new List<string>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                String line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    fileData.Add(line);
                }
            }
            return fileData;
        }
        // this function write the output to the file
        public static void WriteToFile(string fileName, string output)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.WriteLine(output);
            }
        }
        public static int timeComparator((int, int) time1, (int, int) time2)
        {
            if (time1.Item1 < time2.Item1)
                return -1;
            else if (time1.Item1 > time2.Item1)
                return 1;
            else
            {
                if (time1.Item2 < time2.Item2)
                    return -1;
                if (time1.Item2 > time2.Item2)
                    return 1;
            }
            return 0;
        }
        //find and return the maximum interval
        private static int FindMaxInterval(List<(int, int)> duration)
        {
            int sum = 0;
            for (int i = 0; i < duration.Count; i++)
            {
                sum += duration[i].Item2 - duration[i].Item1;
            }
            return sum;
        }
        static void Main(string[] args)
        {
            List<string> inputFile = ReadFromFile("C:\\Users\\Arun Kumar Mahajan\\Downloads\\drive-download-20190718T194404Z-001\\10.in");
            int numberOfGaurds = Int32.Parse(inputFile[0]);
            int maximumInterval = 0;
            List<(int, int)> overLappingIntervals = new List<(int, int)>();
            List<(int, int)> nonoverLappingIntervals = new List<(int, int)>();
            List<(int, int)> gaurdDuration = new List<(int, int)>();
            for (int i = 1; i <= numberOfGaurds; i++)
            {
                string[] times = inputFile[i].Split(" ");
                int startTime = Int32.Parse(times[0]);
                int endTime = Int32.Parse(times[1]);
                gaurdDuration.Add((startTime, endTime));
            }
            gaurdDuration.Sort(timeComparator);
            bool fullOverlap = false;
            // find if there is full overlap of any interval, if yes then remove that interval 
            // and find the maximum interval of the rest of the durations
            for (int i = 0; i < gaurdDuration.Count - 1; i++)
            {
                (int, int) time1 = gaurdDuration[i];
                (int, int) time2 = gaurdDuration[i + 1];
                if (time1.Item1 <= time2.Item1 && time2.Item2 <= time1.Item2)
                {
                    fullOverlap = true;
                    gaurdDuration.RemoveAt(i + 1);
                    i--;
                    // break;
                }
            }
            //segregate overlapping and non-overlapping intervals
            for (int i = 0; i < gaurdDuration.Count; i++)
            {
                (int, int) time1 = gaurdDuration[i];
                if (i == gaurdDuration.Count - 1)
                {
                    nonoverLappingIntervals.Add((time1.Item1, time1.Item2));
                    break;
                }
                (int, int) time2 = gaurdDuration[i + 1];
                //there is an overlap
                if (time2.Item1 < time1.Item2)
                {
                    overLappingIntervals.Add((time2.Item1, time1.Item2));
                    nonoverLappingIntervals.Add((time1.Item1, time2.Item1));
                    gaurdDuration[i + 1] = (time1.Item2, time2.Item2);
                }
                else
                {
                    nonoverLappingIntervals.Add((time1.Item1, time1.Item2));
                }
            }
            if (!fullOverlap)
            {

                //remove the interval with min non-overlapping time
                int minIntervalIndex = -1, minInterval = -1;
                for (int i = 0; i < nonoverLappingIntervals.Count; i++)
                {
                    if (minInterval == -1)
                    {
                        minIntervalIndex = i;
                        minInterval = nonoverLappingIntervals[i].Item2 - nonoverLappingIntervals[i].Item1;
                    }
                    else
                    {
                        int differnce = nonoverLappingIntervals[i].Item2 - nonoverLappingIntervals[i].Item1;
                        if (differnce < minInterval)
                        {
                            minInterval = differnce;
                            minIntervalIndex = i;
                        }
                    }
                }
                if (minIntervalIndex >= 0)
                    nonoverLappingIntervals.RemoveAt(minIntervalIndex);
            }
            // find max interval by adding remaining overlapping and non-overlapping times
            maximumInterval = FindMaxInterval(overLappingIntervals) + FindMaxInterval(nonoverLappingIntervals);

            //output maximumInterval to the output file
            WriteToFile("output10.text", maximumInterval.ToString());
        }
    }
}
