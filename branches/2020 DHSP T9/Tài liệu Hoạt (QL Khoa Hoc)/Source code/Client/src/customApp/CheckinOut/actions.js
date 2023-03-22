import {getRoleByKey} from "../../../helpers/utility";

const actions = {
  CHECKIN_GET_DATA_REQUEST: 'CHECKIN_GET_DATA_REQUEST',
  CHECKIN_GET_DATA_REQUEST_SUCCESS: 'CHECKIN_GET_DATA_REQUEST_SUCCESS',
  CHECKIN_GET_DATA_REQUEST_ERROR: 'CHECKIN_GET_DATA_REQUEST_ERROR',
  getInitData: () => {
    return (disPatch, getState) => {
      let listRole = getState().Auth.role;
      let role = getRoleByKey(listRole, "checkin-out");
      let roleBaoCao = getRoleByKey(listRole, "bao-cao");
      disPatch({
        type: actions.CHECKIN_GET_DATA_REQUEST,
        payload: {role, roleBaoCao}
      });
    }
  }
};
export default actions;