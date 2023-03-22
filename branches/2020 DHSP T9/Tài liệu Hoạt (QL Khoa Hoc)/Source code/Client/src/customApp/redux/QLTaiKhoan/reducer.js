import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  TotalRow: 0,
  TableLoading: false,
  DanhSachNhomNguoiDung: [],
  DanhSachTaiKhoan: [],
  DanhSachTaiKhoanAll: [],
  MatKhauMacDinh: "",
  FileLimit: 1,
  DanhSachCoQuan: []
};

export default function Reducer(state = initState, action) {
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.TAIKHOAN_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.TAIKHOAN_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        TableLoading: false,
        DanhSachTaiKhoan: payload.DanhSachTaiKhoan,
        DanhSachNhomNguoiDung: payload.DanhSachNhomNguoiDung,
        DanhSachTaiKhoanAll: payload.DanhSachTaiKhoanAll,
        DanhSachCoQuanAll: payload.DanhSachCoQuanAll,
        MatKhauMacDinh: payload.MatKhauMacDinh,
        FileLimit: payload.FileLimit,
        DanhSachCoQuan: payload.DanhSachCoQuan,
      };
    case actions.TAIKHOAN_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false,
        DanhSachTaiKhoan: [],
        DanhSachNhomNguoiDung: [],
        DanhSachTaiKhoanAll: [],
        MatKhauMacDinh: "",
        FileLimit: 1,
        DanhSachCoQuan: [],
      };
    //get list
    case actions.TAIKHOAN_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.TAIKHOAN_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        DanhSachTaiKhoan: payload.DanhSachTaiKhoan,
        TableLoading: false
      };
    case actions.TAIKHOAN_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}