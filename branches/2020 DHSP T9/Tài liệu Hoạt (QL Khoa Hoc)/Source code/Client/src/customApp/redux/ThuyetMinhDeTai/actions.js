import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  THUYETMINH_GET_INIT_DATA_REQUEST: 'THUYETMINH_GET_INIT_DATA_REQUEST',
  THUYETMINH_GET_INIT_DATA_REQUEST_SUCCESS: 'THUYETMINH_GET_INIT_DATA_REQUEST_SUCCESS',
  THUYETMINH_GET_INIT_DATA_REQUEST_ERROR: 'THUYETMINH_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("thuyet-minh-de-tai");
      let roleDuyet = getRoleByKey("duyet-thuyet-minh");
      //-------
      disPatch({
        type: actions.THUYETMINH_GET_INIT_DATA_REQUEST,
        payload: {filterData, role, roleDuyet}
      });
    }
  },

  THUYETMINH_GET_LIST_REQUEST: 'THUYETMINH_GET_LIST_REQUEST',
  THUYETMINH_GET_LIST_REQUEST_SUCCESS: 'THUYETMINH_GET_LIST_REQUEST_SUCCESS',
  THUYETMINH_GET_LIST_REQUEST_ERROR: 'THUYETMINH_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.THUYETMINH_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;