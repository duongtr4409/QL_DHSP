import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  LOAIKETQUA_GET_INIT_DATA_REQUEST: 'LOAIKETQUA_GET_INIT_DATA_REQUEST',
  LOAIKETQUA_GET_INIT_DATA_REQUEST_SUCCESS: 'LOAIKETQUA_GET_INIT_DATA_REQUEST_SUCCESS',
  LOAIKETQUA_GET_INIT_DATA_REQUEST_ERROR: 'LOAIKETQUA_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("dm-loai-ket-qua");
      //-------
      disPatch({
        type: actions.LOAIKETQUA_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  LOAIKETQUA_GET_LIST_REQUEST: 'LOAIKETQUA_GET_LIST_REQUEST',
  LOAIKETQUA_GET_LIST_REQUEST_SUCCESS: 'LOAIKETQUA_GET_LIST_REQUEST_SUCCESS',
  LOAIKETQUA_GET_LIST_REQUEST_ERROR: 'LOAIKETQUA_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.LOAIKETQUA_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;