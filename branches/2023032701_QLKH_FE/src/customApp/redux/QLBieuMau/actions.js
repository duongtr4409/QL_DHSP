import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  BIEUMAU_GET_INIT_DATA_REQUEST: 'BIEUMAU_GET_INIT_DATA_REQUEST',
  BIEUMAU_GET_INIT_DATA_REQUEST_SUCCESS: 'BIEUMAU_GET_INIT_DATA_REQUEST_SUCCESS',
  BIEUMAU_GET_INIT_DATA_REQUEST_ERROR: 'BIEUMAU_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("ql-bieu-mau");
      //-------
      disPatch({
        type: actions.BIEUMAU_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  BIEUMAU_GET_LIST_REQUEST: 'BIEUMAU_GET_LIST_REQUEST',
  BIEUMAU_GET_LIST_REQUEST_SUCCESS: 'BIEUMAU_GET_LIST_REQUEST_SUCCESS',
  BIEUMAU_GET_LIST_REQUEST_ERROR: 'BIEUMAU_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.BIEUMAU_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;