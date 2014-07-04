using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICSharpCode.NRefactory.Cpp
{
    class Constants
    {
        public static readonly String IndexerSetter = "SetData";
        public static readonly String IndexerGetter = "GetData";
        public static readonly String ArrayType = "Array";
        public static readonly String ArrayNDType = "ArrayND";
        public static readonly String IsInstanceOf = "is_inst_of";
        public static readonly String AsCast = "as_cast";
        public static readonly String ToStringMethodName = "ToString";
        public static readonly String TypeTraitDeclaration = "TypeDecl";

        public static readonly String DelegateDeclaration = "DELEGATE";
        public static readonly String DelegateFunction = "DELEGATE_FUNC";
        public static readonly String DelegateInvoke = "DELEGATE_INVOKE";
        public static readonly String DelegateType = "Delegate";

        public static readonly String EventDeclaration = "EVENT";
        public static readonly String EventInit = "EVENT_INIT";
        public static readonly String EventFire = "EVENT_FIRE";
        public static readonly String EventType = "Event";

        public static readonly String ExternC = "extern \"C\"";
    }
}