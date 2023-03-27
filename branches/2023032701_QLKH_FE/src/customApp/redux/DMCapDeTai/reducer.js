import actions from "./actions";

const initState = {
  role: { view: 0, add: 0, edit: 0, delete: 0 },
  DanhSachCapDeTai: [],
  DanhSachDiaGioi: [],
  expandedKeys: [],
  TableLoading: false,
  user_id: 1,
  ListPhanQuyenAdmin: [],
};

export default function Reducer(state = initState, action) {
  const { type, payload } = action;
  switch (type) {
    //get initData
    case actions.COQUAN_GET_INIT_DATA_REQUEST:
      return {
        ...state,
        role: payload.role,
        user_id: payload.user_id,
        TableLoading: true,
      };
    case actions.COQUAN_GET_INIT_DATA_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachCapDeTai: payload.DanhSachCapDeTai,
        DanhSachDiaGioi: payload.DanhSachDiaGioi,
        expandedKeys: payload.expandedKeys,
        ListPhanQuyenAdmin: payload.ListPhanQuyenAdmin,
        TableLoading: false,
      };
    case actions.COQUAN_GET_INIT_DATA_REQUEST_ERROR:
      return {
        ...state,
        DanhSachCapDeTai: [],
        expandedKeys: [],
        TableLoading: false,
      };
    //get list
    case actions.COQUAN_GET_LIST_REQUEST:
      return {
        ...state,
        TableLoading: true,
      };
    case actions.COQUAN_GET_LIST_REQUEST_SUCCESS:
      return {
        ...state,
        DanhSachCapDeTai: payload.DanhSachCapDeTai,
        expandedKeys: payload.expandedKeys,
        TableLoading: false,
      };
    case actions.COQUAN_GET_LIST_REQUEST_ERROR:
      return {
        ...state,
        DanhSachCapDeTai: [],
        expandedKeys: [],
        TableLoading: false,
      };
    default:
      return state;
  }
}
