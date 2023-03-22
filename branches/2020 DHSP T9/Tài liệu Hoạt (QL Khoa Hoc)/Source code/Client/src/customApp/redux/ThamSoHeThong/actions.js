import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  THAMSO_GET_INIT_DATA_REQUEST: 'THAMSO_GET_INIT_DATA_REQUEST',
  THAMSO_GET_INIT_DATA_REQUEST_SUCCESS: 'THAMSO_GET_INIT_DATA_REQUEST_SUCCESS',
  THAMSO_GET_INIT_DATA_REQUEST_ERROR: 'THAMSO_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("tham-so");
      //-------
      disPatch({
        type: actions.THAMSO_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  THAMSO_GET_LIST_REQUEST: 'THAMSO_GET_LIST_REQUEST',
  THAMSO_GET_LIST_REQUEST_SUCCESS: 'THAMSO_GET_LIST_REQUEST_SUCCESS',
  THAMSO_GET_LIST_REQUEST_ERROR: 'THAMSO_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.THAMSO_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;