import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  QLNHAKHOAHOC_GET_INIT_DATA_REQUEST: 'QLNHAKHOAHOC_GET_INIT_DATA_REQUEST',
  QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS: 'QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS',
  QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR: 'QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("ql-nha-khoa-hoc");
      let roleLyLich = getRoleByKey("chi-tiet-nha-khoa-hoc");
      disPatch({
        type: actions.QLNHAKHOAHOC_GET_INIT_DATA_REQUEST,
        payload: {filterData, role, roleLyLich}
      });
    }
  },

  QLNHAKHOAHOC_GET_LIST_REQUEST: 'QLNHAKHOAHOC_GET_LIST_REQUEST',
  QLNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS: 'QLNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS',
  QLNHAKHOAHOC_GET_LIST_REQUEST_ERROR: 'QLNHAKHOAHOC_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.QLNHAKHOAHOC_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;