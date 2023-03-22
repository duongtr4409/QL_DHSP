import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  PHANQUYEN_GET_INIT_DATA_REQUEST: 'PHANQUYEN_GET_INIT_DATA_REQUEST',
  PHANQUYEN_GET_INIT_DATA_REQUEST_SUCCESS: 'PHANQUYEN_GET_INIT_DATA_REQUEST_SUCCESS',
  PHANQUYEN_GET_INIT_DATA_REQUEST_ERROR: 'PHANQUYEN_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("phan-quyen");
      //-------
      disPatch({
        type: actions.PHANQUYEN_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  PHANQUYEN_GET_LIST_REQUEST: 'PHANQUYEN_GET_LIST_REQUEST',
  PHANQUYEN_GET_LIST_REQUEST_SUCCESS: 'PHANQUYEN_GET_LIST_REQUEST_SUCCESS',
  PHANQUYEN_GET_LIST_REQUEST_ERROR: 'PHANQUYEN_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.PHANQUYEN_GET_LIST_REQUEST,
    payload: {filterData}
  }),

  PHANQUYEN_DELETE_DATA_REQUEST: 'PHANQUYEN_DELETE_DATA_REQUEST',
  PHANQUYEN_DELETE_DATA_REQUEST_SUCCESS: 'PHANQUYEN_DELETE_DATA_REQUEST_SUCCESS',
  PHANQUYEN_DELETE_DATA_REQUEST_ERROR: 'PHANQUYEN_DELETE_DATA_REQUEST_ERROR',
  deleteData: (idArray) => {
    return (disPatch, getState) => {
      const oldAdTemplateList = getState().AdTemplate.adTemplateList;
      const adTemplateList = oldAdTemplateList.filter(record => !idArray.includes( record.id));
      disPatch({
        type: actions.PHANQUYEN_DELETE_DATA_REQUEST,
        payload: {adTemplateList}
      });
    }
  },
};
export default actions;