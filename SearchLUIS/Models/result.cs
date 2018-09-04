using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchLUIS.Models
{
    public class Result

    {

        public class Rootobject

        {

            public string id { get; set; }

            public string BlobUri { get; set; }

            public string LocalFilePath { get; set; }

            public string FileName { get; set; }

            public string Caption { get; set; }

            public string[] Tags { get; set; }

            public int NumFaces { get; set; }

            public Face[] Faces { get; set; }

        }



        public class Face

        {

            public string UniqueFaceId { get; set; }

            public Facerectangle FaceRectangle { get; set; }

            public string TopEmotion { get; set; }

            public string Gender { get; set; }

            public float Age { get; set; }

        }



        public class Facerectangle

        {

            public int Width { get; set; }

            public int Height { get; set; }

            public int Left { get; set; }

            public int Top { get; set; }

        }



    }

}
