import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  TRANGTHAI_GET_INIT_DATA_REQUEST: 'TRANGTHAI_GET_INIT_DATA_REQUEST',
  TRANGTHAI_GET_INIT_DATA_REQUEST_SUCCESS: 'TRANGTHAI_GET_INIT_DATA_REQUEST_SUCCESS',
  TRANGTHAI_GET_INIT_DATA_REQUEST_ERROR: 'TRANGTHAI_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("dm-trang-thai");
      //-------
      disPatch({
        type: actions.TRANGTHAI_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  TRANGTHAI_GET_LIST_REQUEST: 'TRANGTHAI_GET_LIST_REQUEST',
  TRANGTHAI_GET_LIST_REQUEST_SUCCESS: 'TRANGTHAI_GET_LIST_REQUEST_SUCCESS',
  TRANGTHAI_GET_LIST_REQUEST_ERROR: 'TRANGTHAI_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.TRANGTHAI_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;