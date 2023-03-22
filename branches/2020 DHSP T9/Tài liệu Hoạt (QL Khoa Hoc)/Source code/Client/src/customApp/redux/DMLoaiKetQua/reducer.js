import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  DanhSachLoaiKetQua: [],
  expandedKeys: [],
  TableLoading: false,
  DanhSachNhiemVu: []
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.LOAIKETQUA_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.LOAIKETQUA_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachLoaiKetQua: payload.DanhSachLoaiKetQua,
        expandedKeys: payload.expandedKeys,
        TableLoading: false,
        DanhSachNhiemVu: payload.DanhSachNhiemVu
      };
    case actions.LOAIKETQUA_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachLoaiKetQua: [],
        expandedKeys: [],
        TableLoading: false,
        DanhSachNhiemVu: []
      };
    //get list
    case actions.LOAIKETQUA_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.LOAIKETQUA_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachLoaiKetQua: payload.DanhSachLoaiKetQua,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.LOAIKETQUA_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        DanhSachLoaiKetQua: [],
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}