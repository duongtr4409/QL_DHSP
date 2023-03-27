import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  LOAIHINHNGHIENCUU_GET_INIT_DATA_REQUEST: 'LOAIHINHNGHIENCUU_GET_INIT_DATA_REQUEST',
  LOAIHINHNGHIENCUU_GET_INIT_DATA_REQUEST_SUCCESS: 'LOAIHINHNGHIENCUU_GET_INIT_DATA_REQUEST_SUCCESS',
  LOAIHINHNGHIENCUU_GET_INIT_DATA_REQUEST_ERROR: 'LOAIHINHNGHIENCUU_GET_INIT_DATA_REQUEST_ERROR',
  getInitData: (filterData) => {
    return (disPatch, getState) => {
      //get role
      const roleID = filterData.role;
      let listRole = JSON.parse(localStorage.getItem('list_role'));
      let role = getRoleByKey("dm-loai-hinh-nghien-cuu");
      //-------
      disPatch({
        type: actions.LOAIHINHNGHIENCUU_GET_INIT_DATA_REQUEST,
        payload: {filterData, role}
      });
    }
  },

  LOAIHINHNGHIENCUU_GET_LIST_REQUEST: 'LOAIHINHNGHIENCUU_GET_LIST_REQUEST',
  LOAIHINHNGHIENCUU_GET_LIST_REQUEST_SUCCESS: 'LOAIHINHNGHIENCUU_GET_LIST_REQUEST_SUCCESS',
  LOAIHINHNGHIENCUU_GET_LIST_REQUEST_ERROR: 'LOAIHINHNGHIENCUU_GET_LIST_REQUEST_ERROR',
  getList: (filterData) => ({
    type: actions.LOAIHINHNGHIENCUU_GET_LIST_REQUEST,
    payload: {filterData}
  }),
};
export default actions;