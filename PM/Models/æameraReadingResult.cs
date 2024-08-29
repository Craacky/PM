using System;

namespace PM.Models
{
    public class СameraReadingResult
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string BoxCameraReadingResult { get; set; } = "---";

        public string ProductCameraSlaveReadingResult { get; set; } = "---";

        public string ProductCameraMasterReadingResult { get; set; } = "---";
    }
}
