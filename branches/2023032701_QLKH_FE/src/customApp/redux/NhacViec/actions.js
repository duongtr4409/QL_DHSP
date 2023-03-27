import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  NHACVIEC_GET_DATA_REQUEST: 'NHACVIEC_GET_DATA_REQUEST',
  NHACVIEC_GET_DATA_REQUEST_SUCCESS: 'NHACVIEC_GET_DATA_REQUEST_SUCCESS',
  NHACVIEC_GET_DATA_REQUEST_ERROR: 'NHACVIEC_GET_DATA_REQUEST_ERROR',
  getNotifications: (filterData) => {
    return (disPatch) => {
      disPatch({
        type: actions.NHACVIEC_GET_DATA_REQUEST,
        payload: {filterData}
      });
    }
  }
};
export default actions;