import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  TotalRow: 0,
  TableLoading: false,
  DanhSachHoiDong: [],
  DanhSachCanBo: [],
  defaultPageSize: 10,
  DanhSachCapQuanLy: []
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    case actions.QLHOIDONG_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.QLHOIDONG_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        DanhSachHoiDong: payload.DanhSachHoiDong,
        DanhSachCanBo: payload.DanhSachCanBo,
        DanhSachCapQuanLy: payload.DanhSachCapQuanLy,
        TableLoading: false,
        defaultPageSize: payload.defaultPageSize
      };
    case actions.QLHOIDONG_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false,
        DanhSachHoiDong: [],
        DanhSachCanBo: [],
        DanhSachCapQuanLy: [],
      };
    //get list
    case actions.QLHOIDONG_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.QLHOIDONG_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        DanhSachHoiDong: payload.DanhSachHoiDong,
        TableLoading: false
      };
    case actions.QLHOIDONG_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}