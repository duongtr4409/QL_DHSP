import {Map} from "immutable";
import {store} from "../redux/store";
import moment from "moment";
import {debounce, isArray} from "lodash";
import {fileUploadLimit} from "../settings/constants";
import {message} from "antd";
import optionsSidebarRaw from "../customApp/sidebar";
import optionsSidebarGroup from "../customApp/sidebargroup";
import lodash from "lodash";
import apiConfig from "../customApp/containers/ThamSoHeThong/config";

export function _debounce(callback, time = 300) {
  return debounce(callback, time);
}

export function clearToken() {
  //localStorage.removeItem('id_token');
  localStorage.clear();
}

export function getToken() {
  try {
    const idToken = localStorage.getItem("id_token");
    const userId = localStorage.getItem("user_id");
    const accessToken = localStorage.getItem("access_token");
    return new Map({idToken, userId, accessToken});
  } catch (err) {
    clearToken();
    return new Map();
  }
}

export function isFullLocalStorage() {
  const idToken = localStorage.getItem("id_token");
  const user_id = localStorage.getItem("user_id");
  const access_token = localStorage.getItem("access_token");
  return !(!idToken || !user_id || !access_token);
}

export function timeDifference(givenTime) {
  givenTime = new Date(givenTime);
  const milliseconds = new Date().getTime() - givenTime.getTime();
  const numberEnding = (number) => {
    return number > 1 ? "s" : "";
  };
  const number = (num) => (num > 9 ? "" + num : "0" + num);
  const getTime = () => {
    let temp = Math.floor(milliseconds / 1000);
    const years = Math.floor(temp / 31536000);
    if (years) {
      const month = number(givenTime.getUTCMonth() + 1);
      const day = number(givenTime.getUTCDate());
      const year = givenTime.getUTCFullYear() % 100;
      return `${day}-${month}-${year}`;
    }
    const days = Math.floor((temp %= 31536000) / 86400);
    if (days) {
      if (days < 28) {
        return days + " day" + numberEnding(days);
      } else {
        const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        const month = months[givenTime.getUTCMonth()];
        const day = number(givenTime.getUTCDate());
        return `${day} ${month}`;
      }
    }
    const hours = Math.floor((temp %= 86400) / 3600);
    if (hours) {
      return `${hours} hour${numberEnding(hours)} ago`;
    }
    const minutes = Math.floor((temp %= 3600) / 60);
    if (minutes) {
      return `${minutes} minute${numberEnding(minutes)} ago`;
    }
    return "a few seconds ago";
  };
  return getTime();
}

export function stringToInt(value, defValue = 0) {
  if (!value) {
    return 0;
  } else if (!isNaN(value)) {
    return parseInt(value, 10);
  }
  return defValue;
}

export function stringToPosetiveInt(value, defValue = 0) {
  const val = stringToInt(value, defValue);
  return val > -1 ? val : defValue;
}

export function changeUrlFilter(filter) {
  let url = window.location.origin + window.location.pathname;
  let query_arr = [];
  let _arr = [];

  if (filter !== undefined && filter !== null) {
    let property;
    for (property in filter) {
      if (filter[property] !== undefined && filter[property] !== null && filter[property].toString().trim() !== "") {
        _arr.push({
          key: property,
          value: filter[property].toString().trim(),
        });
      }
    }
  }

  if (_arr.length > 0) {
    _arr.forEach((item) => {
      query_arr.push(item.key + "=" + item.value);
    });
  }

  query_arr.sort();
  if (query_arr.length) {
    url = url + "?" + query_arr.join("&");
  }
  window.history.replaceState(null, null, url);
}

export function getFilterData(oldFilterData, onFilter, onOrder) {
  const DefaultPageSize = getDefaultPageSize();
  let filterData = oldFilterData;
  if (onFilter) {
    let {value, property} = onFilter;
    filterData[property] = value;
    //reset paging
    filterData.PageNumber = "";
    if (filterData.PageSize) {
      filterData.PageNumber = 1;
    }
  } else {
    let {pagination, sorter} = onOrder;
    //paging --
    if (pagination !== {}) {
      let PageNumber = pagination.current;
      let PageSize = pagination.pageSize;
      let CurrentPageSize = DefaultPageSize;
      //get currentPageSize
      if (filterData.PageSize) {
        CurrentPageSize = filterData.PageSize;
      }
      //neu changePageSize -> reset PageNumber = 1
      if (PageSize !== CurrentPageSize) {
        PageNumber = 1;
      }
      filterData = {
        ...filterData,
        PageNumber,
        PageSize,
      };
    }
    //order --
    if (sorter !== {}) {
      let OrderByName = "";
      let OrderByOption = "";
      if (sorter.field && (sorter.order === "ascend" || sorter.order === "descend")) {
        OrderByName = sorter.field;
        OrderByOption = sorter.order === "ascend" ? "asc" : "desc";
      }
      if (OrderByOption !== "asc" && OrderByOption !== "desc") {
        delete filterData.OrderByName;
        delete filterData.OrderByOption;
      } else {
        filterData = {
          ...filterData,
          OrderByName,
          OrderByOption,
        };
      }
    }
  }
  //xoa page info neu la default info: 1, DefaultPageSize
  filterData = {
    ...filterData,
    PageNumber: filterData.PageNumber ? parseInt(filterData.PageNumber) : 1,
    PageSize: filterData.PageSize ? parseInt(filterData.PageSize) : DefaultPageSize,
  };
  if ((filterData.PageNumber === 1 && filterData.PageSize === DefaultPageSize) || !filterData.PageNumber) {
    delete filterData.PageNumber;
    delete filterData.PageSize;
  }
  return filterData;
}

export function getScrollParent(node) {
  if (node.parentElement === null) {
    return node;
  }

  return node.parentElement.scrollHeight > node.clientHeight || node.parentElement.scrollWidth > node.clientWidth ? node.parentElement : getScrollParent(node.parentElement);
}

//Get Role ----------------------------------------------------------------------------------------------------------
export function getRoleByKey(key) {
  let role = {view: 0, add: 0, edit: 0, delete: 0};
  let roleID = localStorage.getItem("role_id");
  let listRole = store.getState().Auth.Roles;
  if (!listRole || listRole.length < 1) {
    let roleStore = localStorage.getItem("list_role");
    listRole = JSON.parse(roleStore);
  }
  const Roles = listRole.filter((item) => item.RoleID == roleID)[0];
  if (!Roles) {
    return role;
  }
  const roleData = Roles.Role;
  const roleFilter = roleData.filter((item) => item.MaChucNang === key)[0];
  if (roleFilter) {
    role.view = roleFilter.Xem;
    role.add = roleFilter.Them;
    role.edit = roleFilter.Sua;
    role.delete = roleFilter.Xoa;
    return role;
  } else {
    return role;
  }
}

export function getRoleByKey2(key) {
  // const listRole = store.getState().Auth.role;
  return getRoleByKey(key);
}

export function getSystemConfig(key) {
  const listConfig = store.getState().App.systemConfig;
  if (listConfig && isArray(listConfig)) {
    const config = listConfig.find((d) => d.ConfigKey === key);
    if (!config) {
      return null;
    }
    return config.ConfigValue;
  }
  return null;
}

export async function checkFilesSize(file) {
  // const listConfig = store.getState().App.systemConfig;
  // const configFileSize = listConfig.find((d) => d.ConfigKey === "FILE_LIMIT");
  const requestConfig = await apiConfig.GetByKey({ConfigKey: "FILE_LIMIT"});
  let configFileSize = requestConfig.data.Data;

  const limitFileSize = configFileSize ? configFileSize.ConfigValue : 10;

  const mbSize = file.size / 1024 / 1024;
  if (mbSize < limitFileSize) {
    return {
      valid: true,
      limitFileSize,
    };
  }
  return {valid: false, limitFileSize};
}

export async function checkFileType(file) {
  const requestConfig = await apiConfig.GetByKey({ConfigKey: "LOAI_FILE"});
  let configFileType = requestConfig.data.Data.ConfigValue;
  const extens = file.name.split(".").pop();
  if (configFileType.includes(extens)) {
    return {
      valid: true,
      fileTypes: configFileType,
    };
  }
  return {
    valid: false,
    fileTypes: configFileType,
  };
}

export function getDefaultPageSize() {
  //get dataConfig tu redux storage
  let dataConfig = store.getState().Auth.dataConfig ? store.getState().Auth.dataConfig : null;
  //get dataConfig tu local storage
  if (!dataConfig) {
    const dataConfigJson = localStorage.getItem("data_config");
    dataConfig = JSON.parse(dataConfigJson);
  }
  let defaultPageSize = 10;
  if (dataConfig && dataConfig.pageSize && [10, 20, 30, 40].indexOf(parseInt(dataConfig.pageSize)) >= 0) {
    defaultPageSize = parseInt(dataConfig.pageSize);
  }
  return defaultPageSize;
}

export function formatAmount(amount) {
  if (isNaN(amount)) {
    return amount;
  }
  return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}

//format dataCoQuan in Saga ------------------------------------------------------------------------------------
export function formatDMCoQuan(DanhSachCoQuan) {
  return DanhSachCoQuan.map((value1, index1) => {
    //-------1
    let title1 = value1.Ten;
    let key1 = `${index1}`;
    let valueSelect1 = `${value1.ID}`;
    let children1 = null;
    if (value1.Children) {
      children1 = value1.Children.map((value2, index2) => {
        //------2
        let title2 = value2.Ten;
        let key2 = `${index1}-${index2}`;
        let valueSelect2 = `${value2.ID}`;
        let children2 = null;
        if (value2.Children) {
          children2 = value2.Children.map((value3, index3) => {
            //------3
            let title3 = value3.Ten;
            let key3 = `${index1}-${index2}-${index3}`;
            let valueSelect3 = `${value3.ID}`;
            let children3 = null;
            return {
              ...value3,
              title: title3,
              key: key3,
              value: valueSelect3,
              children: children3,
            };
          });
        }
        return {
          ...value2,
          title: title2,
          key: key2,
          value: valueSelect2,
          children: children2,
        };
      });
    }
    return {
      ...value1,
      title: title1,
      key: key1,
      value: valueSelect1,
      children: children1,
    };
  });
}

//format danh mục lĩnh vực
export function formatDataTreeSelect(Data, checkStatus = true) {
  if (checkStatus) {
    Data = Data.filter((item) => item.Status);
  }
  return Data.map((value, index) => {
    //-------1
    let title = value.Name;
    let label = `${value.Code} - ${value.Name}`;
    let key = `${index}-${value.Id}`;
    let valueSelect = `${value.Id}`;
    let children = value.Children && value.Children.length > 0 ? renderChildrenTreeSelect(value.Children, index, checkStatus) : null;
    return {
      ...value,
      title: title,
      label: label,
      key: key,
      value: valueSelect,
      children: children,
    };
  });
}

function renderChildrenTreeSelect(children, indexRoot, checkStatus) {
  if (checkStatus) {
    children = children.filter((item) => item.Status);
  }
  return children.map((value, index) => {
    let title = value.Name;
    let label = `${value.Code} - ${value.Name}`;
    let key = `${indexRoot}-${index}-${value.Id}`;
    let valueSelect = `${value.Id}`;
    let children = value.Children && value.Children.length > 0 ? renderChildrenTreeSelect(value.Children, index) : null;
    return {
      ...value,
      title: title,
      label: label,
      key: key,
      value: valueSelect,
      children: children,
    };
  });
}

//Add File -----------------------------------------------------------------------------------------------------
export function getBase64(file, callback) {
  const reader = new FileReader();
  reader.addEventListener("load", () => callback(reader.result));
  reader.readAsDataURL(file);
}

export function beforeUpload(file) {
  const isLimit = file.size / 1024 / 1024 < fileUploadLimit;
  if (!isLimit) {
    message.error("Dung lượng file " + file.name + " đính kèm quá lớn, không thể tải lên");
  }
  return isLimit;
}

export function indexOfObjectInArray(object, array, properties) {
  let indexOf = -1;
  array.forEach((item, index) => {
    if (item[properties] === object[properties]) indexOf = index;
  });
  return indexOf;
}

export const specialPermission = [
  {
    key: "duyet-de-xuat",
    label: "Duyệt đề xuất",
    leftIcon: "appstore",
  },
  {
    key: "de-xuat",
    label: "Gửi đề xuất",
    leftIcon: "appstore",
  },
  {
    key: "ql-toan-truong",
    label: "Quản lý dữ liệu toàn Trường",
    leftIcon: "appstore",
  },
  {
    key: "ql-don-vi",
    label: "Quản lý dữ liệu đơn vị",
    leftIcon: "appstore",
  },
  {
    key: "duyet-thuyet-minh",
    label: "Duyệt thuyết minh",
    leftIcon: "appstore",
  },
];

export function getOptionSidebar() {
  let optionsSidebar = [];
  const optionsSidebarRawClone = JSON.parse(JSON.stringify(optionsSidebarRaw));
  optionsSidebarRawClone.forEach((item) => {
    optionsSidebar.push(item);
  });
  optionsSidebar = [...optionsSidebar, ...specialPermission];
  return optionsSidebar;
}

export function getOptionSidebarGroup() {
  let optionsSidebar = [];
  const optionsSidebarRawClone = JSON.parse(JSON.stringify(optionsSidebarGroup));
  optionsSidebarRawClone.forEach((item) => {
    optionsSidebar.push(item);
  });
  optionsSidebar = [...optionsSidebar, ...specialPermission];
  return optionsSidebar;
}

export function convertFileTable(data = [], selectedField = [], keyField = ["NoiDung", "NgayTao"]) {
  const groupedList = lodash.groupBy(
    data.map((item) => ({...item, ...{key: `${item[keyField[0]]}${item[keyField[1]]}`}})),
    "key"
  );

  const rows = Object.keys(groupedList).map((item) => {
    let row = {
      files: groupedList[item],
    };
    selectedField.forEach((element) => {
      row[element] = groupedList[item][0][element];
    });
    return row;
  });
  return rows;
}

export function checkValidFileName(fileName, listValid) {
  const arrName = fileName.split(".");
  const length = arrName.length;
  const extension = arrName[length - 1];
  return !listValid.includes(extension);
}

export function romanize(num) {
  if (isNaN(num))
    return NaN;
  let digits = String(+num).split(""),
    key = ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM",
      "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC",
      "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"],
    roman = "",
    i = 3;
  while (i--)
    roman = (key[+digits.pop() + (i * 10)] || "") + roman;
  return Array(+digits.join("") + 1).join("M") + roman;
}
