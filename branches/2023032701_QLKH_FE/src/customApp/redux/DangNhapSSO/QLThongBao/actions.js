import { getRoleByKey } from "../../../../helpers/utility";

const actions = {
  SSO_GET_INIT_DATA_REQUEST: "SSO_GET_INIT_DATA_REQUEST",
  SSO_GET_INIT_DATA_REQUEST_SUCCESS: "SSO_GET_INIT_DATA_REQUEST_SUCCESS",
  SSO_GET_INIT_DATA_REQUEST_ERROR: "SSO_GET_INIT_DATA_REQUEST_ERROR",
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("ql-thong-bao");
      //-------
      disPatch({
        type: actions.SSO_GET_INIT_DATA_REQUEST,
        payload: { filterData, role },
      });
    };
  },

  SSO_GET_LIST_REQUEST: "SSO_GET_LIST_REQUEST",
  SSO_GET_LIST_REQUEST_SUCCESS: "SSO_GET_LIST_REQUEST_SUCCESS",
  SSO_GET_LIST_REQUEST_ERROR: "SSO_GET_LIST_REQUEST_ERROR",
  getList: (filterData) => ({
    type: actions.SSO_GET_LIST_REQUEST,
    payload: { filterData },
  }),
};
export default actions;
