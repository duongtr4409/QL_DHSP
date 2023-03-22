import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  DONVINGHIENCUU_GET_INIT_DATA_REQUEST: 'DONVINGHIENCUU_GET_INIT_DATA_REQUEST',
  DONVINGHIENCUU_GET_INIT_DATA_REQUEST_SUCCESS: 'DONVINGHIENCUU_GET_INIT_DATA_REQUEST_SUCCESS',
  DONVINGHIENCUU_GET_INIT_DATA_REQUEST_ERROR: 'DONVINGHIENCUU_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("don-vi-nghien-cuu");
      //-------
      disPatch({
        type: actions.DONVINGHIENCUU_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  DONVINGHIENCUU_GET_LIST_REQUEST: 'DONVINGHIENCUU_GET_LIST_REQUEST',
  DONVINGHIENCUU_GET_LIST_REQUEST_SUCCESS: 'DONVINGHIENCUU_GET_LIST_REQUEST_SUCCESS',
  DONVINGHIENCUU_GET_LIST_REQUEST_ERROR: 'DONVINGHIENCUU_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.DONVINGHIENCUU_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;