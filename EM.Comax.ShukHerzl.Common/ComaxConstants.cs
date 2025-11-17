using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Comax.ShukHerzl.Common
{
    public class ComaxConstants
    {
        public const string PROMOTION_METHOD_URL = "Promotions_Service.asmx/GetPromotionsDef";
        public const string CATALOG_METHOD_URL = "Items_Service.asmx/Get_AllItemsDetailsBySearch";
        public const string CATALOG_METHOD_URL_NEW = "Items_Service.asmx";
        public const string PRICE_METHOD_URL = "Items_Service.asmx";

        // SOAP envelope constants
        public const string SOAP_ENVELOPE_NAMESPACE = "http://schemas.xmlsoap.org/soap/envelope/";
        public const string COMAX_WS_NAMESPACE = "http://ws.comax.co.il/Comax_WebServices/";

        // SOAP action for price search
        public const string SOAP_ACTION_GET_ALL_ITEMS_PRICES = "Get_AllItemsPricesBySearch";

        // XML content type header for SOAP requests
        public const string CONTENT_TYPE_XML = "text/xml";

        // Parameter names (if you want to build the XML dynamically)
        public const string PARAM_ITEM_ID = "ItemID";
        public const string PARAM_DEPARTMENT_ID = "DepartmentID";
        public const string PARAM_GROUP_ID = "GroupID";
        public const string PARAM_SUB_GROUP_ID = "Sub_GroupID";
        public const string PARAM_ITEM_MODEL_ID = "ItemModelID";
        public const string PARAM_ITEM_COLOR_ID = "ItemColorID";
        public const string PARAM_ITEM_SIZE_ID = "ItemSizeID";
        public const string PARAM_STORE_ID = "StoreID";
        public const string PARAM_PRICE_LIST_ID = "PriceListID";
        public const string PARAM_STORE_ID_FOR_OPEN_ORDERS_OFFSET = "StoreIDForOpenOrdersOffset";
        public const string PARAM_SUPPLIER_ID = "SupplierID";
        public const string PARAM_CUSTOMER_ID = "CustomerID";
        public const string PARAM_LAST_UPDATED_FROM_DATE = "LastUpdatedFromDate";
        public const string PARAM_LOGIN_ID = "LoginID";
        public const string PARAM_LOGIN_PASSWORD = "LoginPassword";
        public const string PARAM_CHANGES_AFTER = "ChangesAfter";
    }
}
