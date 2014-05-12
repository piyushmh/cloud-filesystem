using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudServer
{
    public class DataAcess : IDisposable
    {        
        private PhotoManagerEntities data;
                  
        public DataAcess()
        {           
            data = new PhotoManagerEntities();
        }

        public IQueryable<StoredFile> GetFiles()
        {
            var files = from p in data.StoredFiles
                        orderby p.DateTime
                         select p;

            return files;
        }

        public void InsertFile(StoredFile file)
        {
            data.StoredFiles.AddObject(file);
            try
            {
                data.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public StoredFile GetLastFile()
        {
            var lastFile = (from p in data.StoredFiles
                             orderby p.DateTime descending
                             select p).FirstOrDefault();

            return lastFile;
        }

        public StoredFile GetFile(int id)
        {
            var file = (from p in data.StoredFiles
                         where p.FileID == id
                         select p).FirstOrDefault();

            return file;
        }

        public void DeleteFile(int id)
        {
            var file = (from p in data.StoredFiles
                         where p.FileID == id
                         select p).FirstOrDefault();

            if (file != null)
            {
                data.StoredFiles.DeleteObject(file);
                data.SaveChanges();
            }
        }

        public void Dispose()
        {
            data.Dispose();            
        }
      

        public IQueryable<Photo> GetPhotos()
        {
            var photos = from p in data.Photos
                         orderby p.DateTime
                         select p;

            return photos;
        }

        public void InsertPhoto(Photo photo)
        {
            data.Photos.AddObject(photo);
            try
            {
                data.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Photo GetLastPhoto()
        {
            var lastPhoto = (from p in data.Photos
                             orderby p.DateTime descending
                             select p).FirstOrDefault();

            return lastPhoto;
        }

        public Photo GetPhoto(int id)
        {
            var photo = (from p in data.Photos
                         where p.PhotoID == id
                         select p).FirstOrDefault();

            return photo;
        }

        public void DeletePhoto(int id)
        {
            var photo = (from p in data.Photos
                         where p.PhotoID == id
                         select p).FirstOrDefault();

            if (photo != null)
            {
                data.Photos.DeleteObject(photo);
                data.SaveChanges();
            }
        }
    }
}
