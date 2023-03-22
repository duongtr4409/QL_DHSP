import {getRoleByKey} from '../../../helpers/utility';
const actions = {
  COQUAN_GET_INIT_DATA_REQUEST: 'COQUAN_GET_INIT_DATA_REQUEST',
  COQUAN_GET_INIT_DATA_REQUEST_SUCCESS: 'COQUAN_GET_INIT_DATA_REQUEST_SUCCESS',
  COQUAN_GET_INIT_DATA_REQUEST_ERROR: 'COQUAN_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("co-quan-don-vi");
      let user_id = parseInt(localStorage.getItem('user_id'));
      // -------
      disPatch({
        type: actions.COQUAN_GET_INIT_DATA_REQUEST,
        payload: {filterData, role, user_id}
      });
    }
  },

  COQUAN_GET_LIST_REQUEST: 'COQUAN_GET_LIST_REQUEST',
  COQUAN_GET_LIST_REQUEST_SUCCESS: 'COQUAN_GET_LIST_REQUEST_SUCCESS',
  COQUAN_GET_LIST_REQUEST_ERROR: 'COQUAN_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.COQUAN_GET_LIST_REQUEST,
    payload: {filterData}
  }),

  COQUAN_DELETE_DATA_REQUEST: 'COQUAN_DELETE_DATA_REQUEST',
  COQUAN_DELETE_DATA_REQUEST_SUCCESS: 'COQUAN_DELETE_DATA_REQUEST_SUCCESS',
  COQUAN_DELETE_DATA_REQUEST_ERROR: 'COQUAN_DELETE_DATA_REQUEST_ERROR',
  deleteData: (idArray) => {
    return (disPatch, getState) => {
      const oldAdTemplateList = getState().AdTemplate.adTemplateList;
      const adTemplateList = oldAdTemplateList.filter(record => !idArray.includes( record.id));
      disPatch({
        type: actions.COQUAN_DELETE_DATA_REQUEST,
        payload: {adTemplateList}
      });
    }
  },
};
export default actions;