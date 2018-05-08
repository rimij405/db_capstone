using System;
using System.Collections.Generic;
using System.Linq;
/*
    IBusinessCodes.cs
    ---
    Ian Effendi
 */

using System.Text;
using System.Threading.Tasks;

using ISTE.DAL.Models.Interfaces;

namespace ISTE.BAL.Interfaces
{

    public abstract class BusinessCodeModel<TCodeModel> : IBusinessCodeModel<TCodeModel> where TCodeModel : ICodeModel
    {
        protected TCodeModel codeModel;

        public string Code
        {
            get { return this.codeModel.Code.SQLValue; }
        }

        public IDatabaseObjectModel Model
        {
            get {
                return codeModel;
            }
        }

        protected BusinessCodeModel(TCodeModel model)
        {
            this.codeModel = model;
        }


    }

    public abstract class BusinessCodeItem<TCodeItem> : BusinessCodeModel<TCodeItem>, IBusinessCodeItem<TCodeItem> where TCodeItem : ICodeItem
    {
        protected TCodeItem codeItem;

        public string Name
        {
            get { return this.codeItem.Name; }
        }

        public string Description
        {
            get { return this.codeItem.Description; }
        }

        protected BusinessCodeItem(TCodeItem item)
            : base(item)
        {
            this.codeItem = item;
        }       

    }

    public abstract class BusinessCodes<TModel, TCode> : IBusinessCodes<TModel, TCode> where TModel : ICodeGlossary<TCode> where TCode : ICodeItem
    {
        protected TModel glossary;

        public TModel Glossary
        {
            get { return this.glossary; }
        }
    }

    /// <summary>
    /// An individual code object.
    /// </summary>
    public interface IBusinessCodeModel<T> : IBusinessObject where T : ICodeModel
    {

        /// <summary>
        /// Get code associated with the model.
        /// </summary>
        string Code { get; }

    }

    public interface IBusinessCodeItem<T> : IBusinessCodeModel<T> where T : ICodeItem
    {

        /// <summary>
        /// Get the name associated with the code.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get description associated with code.
        /// </summary>
        string Description { get; }

    }

    /// <summary>
    /// Collection of business codes.
    /// </summary>
    public interface IBusinessCodes<TModel, TCode> where TModel : ICodeGlossary<TCode> where TCode : ICodeItem
    {

        /// <summary>
        /// Collection of codes.
        /// </summary>
        TModel Glossary
        {
            get;
        }

    }

    public interface IBusinessStatusType : IBusinessCodeItem<IStatusModel> {}

    public interface IBusinessEmailType : IBusinessCodeItem<IEmailTypeModel> {}

    public interface IBusinessPhoneType : IBusinessCodeItem<IPhoneTypeModel> {}

    public interface IBusinessRoleType : IBusinessCodeItem<IRoleTypeModel> {}



}
