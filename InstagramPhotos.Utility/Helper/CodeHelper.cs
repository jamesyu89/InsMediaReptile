using System.Collections.Generic;
using System.Linq;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// Code 公共类
    /// </summary>
    public class CodeHelper
    {
        /// <summary>
        /// 平台集合
        /// </summary>
        public CodeList<Code> webIds { get; set; }

        /// <summary>
        /// 状态集合
        /// </summary>
        public CodeList<Code> states { get; set; }

        public CodeList<Code> areaCode { get; set; }
        public CodeList<Code> sendoutType { get; set; }
        public CodeList<Code> storageType { get; set; }
        public CodeList<Code> state { get; set; }
        public CodeList<Code> deliveryCode { get; set; }
        public CodeList<Code> packageType { get; set; }
        public CodeList<Code> commodityDeliveryTime { get; set; }
        public CodeList<Code> commodityType { get; set; }
        public CodeList<Code> commodityTag { get; set; }
        public CodeList<Code> promotionsTypeList { get; set; }

        public CodeList<Code> displayRangeList { get; set; }
        public CodeList<Code> showAreaCode { get; set; }
        public CodeList<Code> adAreaCode { get; set; }

        public CodeList<Code> attributeTypeList { get; set; }

        public CodeList<Code> usedByList { get; set; }

        public CodeList<Code> boolList { get; set; }

        public CodeList<Code> groupBuyTypeList { get; set; }
        public CodeList<Code> taxRateList { get; set; }
        public CodeList<Code> informationType { get; set; }
        public CodeList<Code> adChannel { get; set; }
        public CodeList<Code> salePlatform { get; set; }
        public CodeList<Code> adGroup { get; set; }
        public CodeList<Code> controlType { get; set; }
        public CodeList<Code> wfDataType { get; set; }

        public CodeList<Code> kidTypes { get; set; }
    }

    public class Code
    {
        public decimal Id { get; set; }
        public string Text { get; set; }
    }

    /// <summary>
    /// code集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CodeList<T> : List<T> where T : Code
    {
        /// <summary>
        /// 获取text 值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetText(string id)
        {
            decimal _id;
            if (!decimal.TryParse(id, out _id))
            {
                _id = -999;
            }
            var obj = this.FirstOrDefault(a => a.Id == _id);
            if (obj == null)
            {
                return null;
            }
            return obj.Text;
        }
    }
}
