namespace InstagramPhotos.CodeGender.Coder
{
  public class FunctionOption
    {

      public FunctionOption(string entityClass, string EntitiesCacheName, Column idColumn, 
                           string DalClassName, bool isBat, bool IsBllSinglon,bool IsPartial,bool WithTran)
      {
          this.EntityClass = entityClass;
          this.EntityCacheName = EntitiesCacheName;
          this.IdColumn = idColumn;
          this.DalClass = DalClassName;

          this.IsBat = isBat;
          this.IsBllSingleton = IsBllSinglon;
          this.IsPartial = IsPartial;
          this.WithTran = WithTran;
      }

      public string EntityClass { get; set; }

      public string FunctionName { get; set; }

      public string FunctionName2 { get; set; }

      public string EntityCacheName { get; set; }

      public Column IdColumn { get; set; }

      public bool IsBllSingleton { get; set; }

      public bool IsPartial { get; set; }

      public bool IsBat { get; set; }

      public bool WithTran { get; set; }

      public string ReturnType{ get; set; }

      public string DalClass { get; set; }
      
      
    }
}
