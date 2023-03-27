import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  BUOCTHUCHIEN_GET_INIT_DATA_REQUEST: 'BUOCTHUCHIEN_GET_INIT_DATA_REQUEST',
  BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_SUCCESS: 'BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_SUCCESS',
  BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_ERROR: 'BUOCTHUCHIEN_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("buoc-thuc-hien");
      //-------
      disPatch({
        type: actions.BUOCTHUCHIEN_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  BUOCTHUCHIEN_GET_LIST_REQUEST: 'BUOCTHUCHIEN_GET_LIST_REQUEST',
  BUOCTHUCHIEN_GET_LIST_REQUEST_SUCCESS: 'BUOCTHUCHIEN_GET_LIST_REQUEST_SUCCESS',
  BUOCTHUCHIEN_GET_LIST_REQUEST_ERROR: 'BUOCTHUCHIEN_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.BUOCTHUCHIEN_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;