import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  QLHOIDONG_GET_INIT_DATA_REQUEST: 'QLHOIDONG_GET_INIT_DATA_REQUEST',
  QLHOIDONG_GET_INIT_DATA_REQUEST_SUCCESS: 'QLHOIDONG_GET_INIT_DATA_REQUEST_SUCCESS',
  QLHOIDONG_GET_INIT_DATA_REQUEST_ERROR: 'QLHOIDONG_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("ql-hoi-dong");
      disPatch({
        type: actions.QLHOIDONG_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  QLHOIDONG_GET_LIST_REQUEST: 'QLHOIDONG_GET_LIST_REQUEST',
  QLHOIDONG_GET_LIST_REQUEST_SUCCESS: 'QLHOIDONG_GET_LIST_REQUEST_SUCCESS',
  QLHOIDONG_GET_LIST_REQUEST_ERROR: 'QLHOIDONG_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.QLHOIDONG_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;