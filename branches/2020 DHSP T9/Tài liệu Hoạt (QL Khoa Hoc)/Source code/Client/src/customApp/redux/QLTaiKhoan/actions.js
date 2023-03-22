import { getRoleByKey } from "../../../helpers/utility";

const actions = {
  TAIKHOAN_GET_INIT_DATA_REQUEST: "TAIKHOAN_GET_INIT_DATA_REQUEST",
  TAIKHOAN_GET_INIT_DATA_REQUEST_SUCCESS: "TAIKHOAN_GET_INIT_DATA_REQUEST_SUCCESS",
  TAIKHOAN_GET_INIT_DATA_REQUEST_ERROR: "TAIKHOAN_GET_INIT_DATA_REQUEST_ERROR",
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("ht-tai-khoan");
      disPatch({
        type: actions.TAIKHOAN_GET_INIT_DATA_REQUEST,
        payload: { filterData, role },
      });
    };
  },

  TAIKHOAN_GET_LIST_REQUEST: "TAIKHOAN_GET_LIST_REQUEST",
  TAIKHOAN_GET_LIST_REQUEST_SUCCESS: "TAIKHOAN_GET_LIST_REQUEST_SUCCESS",
  TAIKHOAN_GET_LIST_REQUEST_ERROR: "TAIKHOAN_GET_LIST_REQUEST_ERROR",
  getList: (filterData) => ({
    type: actions.TAIKHOAN_GET_LIST_REQUEST,
    payload: { filterData },
  }),
};
export default actions;
