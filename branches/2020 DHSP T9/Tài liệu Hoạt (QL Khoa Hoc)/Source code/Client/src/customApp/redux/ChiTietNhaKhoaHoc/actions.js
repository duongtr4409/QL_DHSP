import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST: 'CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST',
  CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS: 'CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS',
  CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR: 'CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("chi-tiet-nha-khoa-hoc");
      let roleQL = getRoleByKey("ql-nha-khoa-hoc");
      disPatch({
        type: actions.CHITIETNHAKHOAHOC_GET_INIT_DATA_REQUEST,
        payload: {filterData, role, roleQL}
      });
    }
  },

  CHITIETNHAKHOAHOC_GET_LIST_REQUEST: 'CHITIETNHAKHOAHOC_GET_LIST_REQUEST',
  CHITIETNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS: 'CHITIETNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS',
  CHITIETNHAKHOAHOC_GET_LIST_REQUEST_ERROR: 'CHITIETNHAKHOAHOC_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.CHITIETNHAKHOAHOC_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;