import actions from './actions';

const initState = {
  DanhSachCanBo: [],
  IPCamera: "",
  role: {view: 0, add: 0, edit: 0, delete: 0},
  roleBaoCao: {view: 0, add: 0, edit: 0, delete: 0},
};

export default function Reducer(state = initState, action) {
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.CHECKIN_GET_DATA_REQUEST:
      return {
        ...state,
        role: payload.role,
        roleBaoCao: payload.roleBaoCao,
      };
    case actions.CHECKIN_GET_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachCanBo: payload.DanhSachCanBo,
        IPCamera: payload.IPCamera,
      };
    case actions.CHECKIN_GET_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachCanBo: [],
      };
    default:
      return state;
  }
}