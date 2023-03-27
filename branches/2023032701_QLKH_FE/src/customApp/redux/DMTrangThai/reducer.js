import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  DanhSachTrangThai: [],
  TotalRow: 0,
  TableLoading: false,
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.TRANGTHAI_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.TRANGTHAI_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachTrangThai: payload.DanhSachTrangThai,
        TotalRow: payload.TotalRow,
        TableLoading: false,
      };
    case actions.TRANGTHAI_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachTrangThai: [],
        TotalRow: 0,
        TableLoading: false,
      };
    //get list
    case actions.TRANGTHAI_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.TRANGTHAI_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachTrangThai: payload.DanhSachTrangThai,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.TRANGTHAI_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        DanhSachTrangThai: [],
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}