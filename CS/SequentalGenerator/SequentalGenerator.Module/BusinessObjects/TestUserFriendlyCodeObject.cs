﻿using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SequentalGenerator.Module.BusinessObjects {
    [DefaultClassOptions]
    public class TestUserFriendlyCodeObject : BaseObject {
        public TestUserFriendlyCodeObject(Session session) : base(session) { }
        [NonCloneable, RuleUniqueValue, Indexed(Unique = true)]
        public string Code {
            get { return GetPropertyValue<string>("Code"); }
            set { SetPropertyValue<string>("Code", value); }
        }
        protected override void OnSaving() {
            if (!(Session is NestedUnitOfWork)
                && (Session.DataLayer != null)
                    && Session.IsNewObject(this)
                        && (Session.ObjectLayer is SimpleObjectLayer)
                        //OR
                        //&& !(Session.ObjectLayer is DevExpress.ExpressApp.Security.ClientServer.SecuredSessionObjectLayer)
                        && string.IsNullOrEmpty(Code)) {
                int nextSequence = DistributedIdGeneratorHelper.Generate(Session.DataLayer, this.GetType().FullName, "MyServerPrefix");
                Code = string.Format("N{0:D6}", nextSequence);
            }
            base.OnSaving();
        }
    }
}
