//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.34209
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ruico.Application.Resources.Generated {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class WeixinMessagesResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal WeixinMessagesResources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ruico.Application.Resources.Generated.WeixinMessagesResources", typeof(WeixinMessagesResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 菜单KEY值 的本地化字符串。
        /// </summary>
        internal static string Key {
            get {
                return ResourceManager.GetString("Key", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 应用Id值 的本地化字符串。
        /// </summary>
        internal static string AppId {
            get {
                return ResourceManager.GetString("AppId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 父级菜单 的本地化字符串。
        /// </summary>
        internal static string Parent {
            get {
                return ResourceManager.GetString("Parent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 添加菜单 的本地化字符串。
        /// </summary>
        internal static string Add_AppMenu {
            get {
                return ResourceManager.GetString("Add_AppMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 更新菜单 的本地化字符串。
        /// </summary>
        internal static string Update_AppMenu {
            get {
                return ResourceManager.GetString("Update_AppMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 删除菜单 的本地化字符串。
        /// </summary>
        internal static string Remove_AppMenu {
            get {
                return ResourceManager.GetString("Remove_AppMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 菜单'{0}'已存在 的本地化字符串。
        /// </summary>
        internal static string AppMenu_Exists_WithValue {
            get {
                return ResourceManager.GetString("AppMenu_Exists_WithValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 菜单不存在 的本地化字符串。
        /// </summary>
        internal static string AppMenu_NotExists {
            get {
                return ResourceManager.GetString("AppMenu_NotExists", resourceCulture);
            }
        }
    }
}
