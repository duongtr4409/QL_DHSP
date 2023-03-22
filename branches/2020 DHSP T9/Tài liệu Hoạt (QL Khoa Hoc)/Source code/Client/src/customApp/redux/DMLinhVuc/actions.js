import { getRoleByKey } from "../../../helpers/utility";
const actions = {
  LINHVUC_GET_INIT_DATA_REQUEST: "LINHVUC_GET_INIT_DATA_REQUEST",
  LINHVUC_GET_INIT_DATA_REQUEST_SUCCESS: "LINHVUC_GET_INIT_DATA_REQUEST_SUCCESS",
  LINHVUC_GET_INIT_DATA_REQUEST_ERROR: "LINHVUC_GET_INIT_DATA_REQUEST_ERROR",
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      let role = getRoleByKey("co-quan-don-vi");
      let user_id = parseInt(localStorage.getItem("user_id"));
      // -------
      disPatch({
        type: actions.LINHVUC_GET_INIT_DATA_REQUEST,
        payload: { filterData, role, user_id },
      });
    };
  },

  LINHVUC_GET_LIST_REQUEST: "LINHVUC_GET_LIST_REQUEST",
  LINHVUC_GET_LIST_REQUEST_SUCCESS: "LINHVUC_GET_LIST_REQUEST_SUCCESS",
  LINHVUC_GET_LIST_REQUEST_ERROR: "LINHVUC_GET_LIST_REQUEST_ERROR",
  getList: (filterData) => ({
    type: actions.LINHVUC_GET_LIST_REQUEST,
    payload: { filterData },
  }),

  LINHVUC_DELETE_DATA_REQUEST: "LINHVUC_DELETE_DATA_REQUEST",
  LINHVUC_DELETE_DATA_REQUEST_SUCCESS: "LINHVUC_DELETE_DATA_REQUEST_SUCCESS",
  LINHVUC_DELETE_DATA_REQUEST_ERROR: "LINHVUC_DELETE_DATA_REQUEST_ERROR",
  deleteData: (idArray) => {
    return (disPatch, getState) => {
      const oldAdTemplateList = getState().AdTemplate.adTemplateList;
      const adTemplateList = oldAdTemplateList.filter((record) => !idArray.includes(record.id));
      disPatch({
        type: actions.LINHVUC_DELETE_DATA_REQUEST,
        payload: { adTemplateList },
      });
    };
  },
};
export default actions;
