import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  roleLyLich: {view: 0, add: 0, edit: 0, delete: 0},
  TotalRow: 0,
  TableLoading: false,
  DanhSachNhaKhoaHoc: [],
  FileLimit: 1,
  FileAllow: "",
  DanhSachChucDanh: [],
  DanhSachHocVi: []
};

export default function Reducer(state = initState, action) {
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.QLNHAKHOAHOC_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role,
        roleLyLich: payload.roleLyLich
      };
    case actions.QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        TableLoading: false,
        DanhSachNhaKhoaHoc: payload.DanhSachNhaKhoaHoc,
        FileLimit: payload.FileLimit,
        DanhSachChucDanh: payload.DanhSachChucDanh,
        DanhSachHocVi: payload.DanhSachHocVi,
        FileAllow: payload.FileAllow
      };
    case actions.QLNHAKHOAHOC_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false,
        DanhSachNhaKhoaHoc: [],
        FileLimit: 1,
        DanhSachChucDanh: [],
        DanhSachHocVi: [],
        FileAllow: ""
      };
    //get list
    case actions.QLNHAKHOAHOC_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.QLNHAKHOAHOC_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        TotalRow: payload.TotalRow,
        TableLoading: false,
        DanhSachNhaKhoaHoc: payload.DanhSachNhaKhoaHoc
      };
    case actions.QLNHAKHOAHOC_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        TotalRow: 0,
        TableLoading: false,
        DanhSachNhaKhoaHoc: []
      };
    default:
      return state;
  }
}