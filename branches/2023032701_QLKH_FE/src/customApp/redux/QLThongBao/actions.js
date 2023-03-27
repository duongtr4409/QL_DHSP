import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  THONGBAO_GET_INIT_DATA_REQUEST: 'THONGBAO_GET_INIT_DATA_REQUEST',
  THONGBAO_GET_INIT_DATA_REQUEST_SUCCESS: 'THONGBAO_GET_INIT_DATA_REQUEST_SUCCESS',
  THONGBAO_GET_INIT_DATA_REQUEST_ERROR: 'THONGBAO_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("ql-thong-bao");
      //-------
      disPatch({
        type: actions.THONGBAO_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  THONGBAO_GET_LIST_REQUEST: 'THONGBAO_GET_LIST_REQUEST',
  THONGBAO_GET_LIST_REQUEST_SUCCESS: 'THONGBAO_GET_LIST_REQUEST_SUCCESS',
  THONGBAO_GET_LIST_REQUEST_ERROR: 'THONGBAO_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.THONGBAO_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;