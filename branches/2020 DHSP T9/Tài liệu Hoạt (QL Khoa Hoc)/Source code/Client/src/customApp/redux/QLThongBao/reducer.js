import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  DanhSachThongBao: [],
  TotalRow: 0,
  TableLoading: false,
  DanhSachCanBo: []
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.THONGBAO_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.THONGBAO_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachThongBao: payload.DanhSachThongBao,
        DanhSachCanBo: payload.DanhSachCanBo,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.THONGBAO_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachThongBao: [],
        DanhSachCanBo: [],
        TotalRow: 0,
        TableLoading: false
      };
    //get list
    case actions.THONGBAO_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.THONGBAO_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachThongBao: payload.DanhSachThongBao,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.THONGBAO_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        DanhSachThongBao: [],
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}