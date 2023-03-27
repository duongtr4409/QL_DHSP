import actions from './actions';

const initState = {
  role: {view: 0, add: 0, edit: 0, delete: 0},
  DanhSachDonViNghienCuu: [],
  DanhSachLinhVuc: [],
  TotalRow: 0,
  TableLoading: false
};

export default function Reducer(state = initState, action){
  const {type, payload} = action;
  switch (type) {
    //get initData
    case actions.DONVINGHIENCUU_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        TableLoading: true,
        role: payload.role
      };
    case actions.DONVINGHIENCUU_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachDonViNghienCuu: payload.DanhSachDonViNghienCuu,
        DanhSachLinhVuc: payload.DanhSachLinhVuc,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.DONVINGHIENCUU_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachDonViNghienCuu: [],
        DanhSachLinhVuc: [],
        TotalRow: 0,
        TableLoading: false
      };
    //get list
    case actions.DONVINGHIENCUU_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true
      };
    case actions.DONVINGHIENCUU_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachDonViNghienCuu: payload.DanhSachDonViNghienCuu,
        TotalRow: payload.TotalRow,
        TableLoading: false
      };
    case actions.DONVINGHIENCUU_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        DanhSachDonViNghienCuu: [],
        TotalRow: 0,
        TableLoading: false
      };
    default:
      return state;
  }
}