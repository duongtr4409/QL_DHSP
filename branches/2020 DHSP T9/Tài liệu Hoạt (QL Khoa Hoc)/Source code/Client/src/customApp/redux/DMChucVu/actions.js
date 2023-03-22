import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  CHUCVU_GET_INIT_DATA_REQUEST: 'CHUCVU_GET_INIT_DATA_REQUEST',
  CHUCVU_GET_INIT_DATA_REQUEST_SUCCESS: 'CHUCVU_GET_INIT_DATA_REQUEST_SUCCESS',
  CHUCVU_GET_INIT_DATA_REQUEST_ERROR: 'CHUCVU_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("chuc-vu");
      //-------
      disPatch({
        type: actions.CHUCVU_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  CHUCVU_GET_LIST_REQUEST: 'CHUCVU_GET_LIST_REQUEST',
  CHUCVU_GET_LIST_REQUEST_SUCCESS: 'CHUCVU_GET_LIST_REQUEST_SUCCESS',
  CHUCVU_GET_LIST_REQUEST_ERROR: 'CHUCVU_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.CHUCVU_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;