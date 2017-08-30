using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstagramPhotos.Utility.UmengPush.Base;

namespace InstagramPhotos.Utility.UmengPush.Core
{
    /// <summary>
    /// 发送到友盟的json实体类
    /// </summary>
    public class PostUMengJson
    {

        /// <summary>
        /// 必填 应用唯一标识
        /// </summary>
        public string appkey { get; set; }
        /// <summary>
        /// 注意：该值由UMengMessagePush自动生成，无需主动赋值
        /// 
        /// 必填 时间戳，10位或者13位均可，时间戳有效期为10分钟 
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 必填 消息发送类型,其值可以为:
        /// <example>
        ///unicast-单播
        ///listcast-列播(要求不超过500个device_token)
        ///filecast-文件播
        ///(多个device_token可通过文件形式批量发送）
        ///broadcast-广播
        ///groupcast-组播
        ///(按照filter条件筛选特定用户群, 具体请参照filter参数)
        ///customizedcast(通过开发者自有的alias进行推送), 
        ///包括以下两种case:
        ///- alias: 对单个或者多个alias进行推送
        ///- file_id: 将alias存放到文件后，根据file_id来推送
        ///</example>
        /// </summary>
        public string type { get; set; }

        public string device_tokens { get; set; }
        /// <summary>
        /// 可选 
        /// 当type=customizedcast时必填，alias的类型, 
        /// alias_type可由开发者自定义,
        /// 开发者在SDK中调用setAlias(alias, alias_type)时所设置的alias_type
        /// </summary>
        public string alias_type { get; set; }
        /// <summary>
        /// 可选 当type=customizedcast时, 
        /// 开发者填写自己的alias。 要求不超过50个alias,多个alias以英文逗号间隔。
        /// 在SDK中调用setAlias(alias, alias_type)时所设置的alias
        /// </summary>
        public string alias { get; set; }
        /// <summary>
        ///  可选 当type=filecast时，file内容为多条device_token, 
        ///  device_token以回车符分隔
        ///  当type=customizedcast时，file内容为多条alias，
        ///  alias以回车符分隔，注意同一个文件内的alias所对应
        ///  的alias_type必须和接口参数alias_type一致。
        ///  注意，使用文件播前需要先调用文件上传接口获取file_id, 
        ///  具体请参照"2.4文件上传接口"
        /// </summary>
        public string file_id { get; set; }
        /// <summary>
        /// 可选 终端用户筛选条件,如用户标签、地域、应用版本以及渠道等,
        /// </summary>
        public string filter { get; set; }
        /// <summary>
        /// 必填 消息内容(Android最大为1840B),
        /// </summary>
        public IOS_PayLoad payload { get; set; }
        /// <summary>
        /// 可选 发送策略
        /// </summary>
        public Policy policy { get; set; }

        public string production_mode { get; set; }
        /// <summary>
        /// 可选 发送消息描述，建议填写。
        /// </summary>
        public string description { get; set; }
        public string thirdparty_id { get; set; }
    }

    /// <summary>
    /// 安卓
    /// </summary>
    public class Payload
    {
        /// <summary>
        /// 必填 消息类型，值可以为:notification-通知，message-消息
        /// </summary>
        public string display_type { get; set; }
        /// <summary>
        /// 必填 消息体
        /// </summary>
        public ContentBody body { get; set; }

        /// <summary>
        /// 可选 用户自定义key-value。只对"通知"(display_type=notification)生效。
        /// 可以配合通知到达后,打开App,打开URL,打开Activity使用。
        /// </summary>
        public SerializableDictionary<string, string> extra { get; set; }
    }

    public class IOS_PayLoad
    {
        public APS aps { get; set; }
    }

    public class APS
    {
        public string alert { get; set; }
        public string badge { get; set; }
        public string sound { get; set; }
        private APS(){}

        public APS(string alert)
        {
            this.alert = alert;
        }
    }

    public class ContentBody
    {
        /// <summary>
        /// 必填 通知栏提示文字
        /// </summary>
        public string ticker { get; set; }
        /// <summary>
        /// 必填 通知标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 必填 通知文字描述
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 可选 状态栏图标ID, R.drawable.[smallIcon],
        /// 如果没有, 默认使用应用图标。
        /// 图片要求为24*24dp的图标,或24*24px放在drawable-mdpi下。
        /// 注意四周各留1个dp的空白像素
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 可选 通知栏拉开后左侧图标ID, R.drawable.[largeIcon].
        /// 图片要求为64*64dp的图标,
        /// 可设计一张64*64px放在drawable-mdpi下,
        /// 注意图片四周留空，不至于显示太拥挤
        /// </summary>
        public string largeIcon { get; set; }
        /// <summary>
        /// 可选 通知栏大图标的URL链接。该字段的优先级大于largeIcon。
        /// 该字段要求以http或者https开头。
        /// </summary>
        public string img { get; set; }
        /// <summary>
        /// 可选 通知声音，R.raw.[sound].
        /// 如果该字段为空，采用SDK默认的声音, 即res/raw/下的
        /// umeng_push_notification_default_sound声音文件
        /// 如果SDK默认声音文件不存在，
        /// 则使用系统默认的Notification提示音。
        /// </summary>
        public string sound { get; set; }
        /// <summary>
        /// 可选 默认为0，用于标识该通知采用的样式。使用该参数时, 
        /// 开发者必须在SDK里面实现自定义通知栏样式。
        /// </summary>
        public int builder_id { get; set; }
        /// <summary>
        /// 通知到达设备后的提醒方式（取值为字符串"true/false"）：可选 收到通知是否震动,默认为"true".
        /// </summary>
        public string play_vibrate { get; set; }
        /// <summary>
        /// 可选 收到通知是否闪灯,默认为"true"
        /// </summary>
        public string play_lights { get; set; }
        /// <summary>
        /// 可选 收到通知是否发出声音,默认为"true"
        /// </summary>
        public string play_sound { get; set; }
        /// <summary>
        /// 必填 点击"通知"的后续行为，默认为打开app。
        /// 取值：
        /// "go_app": 打开应用
        /// "go_url": 跳转到URL
        /// "go_activity": 打开特定的activity
        /// "go_custom": 用户自定义内容。
        /// </summary>
        public string after_open { get; set; }
        /// <summary>
        /// 可选 当"after_open"为"go_url"时，必填。通知栏点击后跳转的URL，要求以http或者https开头 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 可选 当"after_open"为"go_activity"时，必填。通知栏点击后打开的Activity
        /// </summary>
        public string activity { get; set; }
        /// <summary>
        /// 可选 display_type=message, 或者display_type=notification且 "after_open"为"go_custom"时，该字段必填。用户自定义内容, 可以为字符串或者JSON格式。
        /// </summary>
        public string custom { get; set; }
    }

    public class Policy
    {
        public string start_time { get; set; }
        public string expire_time { get; set; }
        public int max_send_num { get; set; }
        public string out_biz_no { get; set; }
    }
}
