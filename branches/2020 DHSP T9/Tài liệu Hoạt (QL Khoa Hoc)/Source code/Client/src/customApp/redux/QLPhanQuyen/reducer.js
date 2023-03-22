import actions from './actions';

const initState = {
    role: {view: 0, add: 0, edit: 0, delete: 0},
    DanhSachNhom: [],
    DanhSachCoQuan: [],
    DanhSachCanBo: [],
    //DanhSachChucNang: [],
    TotalRow: 0,
    TableLoading: false,
};

export default function Reducer(state = initState, action){
    const {type, payload} = action;
    switch (type) {
        //get initData
        case actions.PHANQUYEN_GET_INIT_DATA_REQUEST:
            return {
                ...state,
                TableLoading: true,
                role: payload.role
            };
        case actions.PHANQUYEN_GET_INIT_DATA_REQUEST_SUCCESS:
            return {
                ...state,
                DanhSachNhom: payload.DanhSachNhom,
                DanhSachCoQuan: payload.DanhSachCoQuan,
                DanhSachCanBo: payload.DanhSachCanBo,
                //DanhSachChucNang: payload.DanhSachChucNang,
                TotalRow: payload.TotalRow,
                TableLoading: false
            };
        case actions.PHANQUYEN_GET_INIT_DATA_REQUEST_ERROR:
            return {
                ...state,
                DanhSachNhom: [],
                TotalRow: 0,
                TableLoading: false
            };
        //get list
        case actions.PHANQUYEN_GET_LIST_REQUEST:
            return {
                ...state,
                TableLoading: true
            };
        case actions.PHANQUYEN_GET_LIST_REQUEST_SUCCESS:
            return {
                ...state,
                DanhSachNhom: payload.DanhSachNhom,
                DanhSachCanBo: payload.DanhSachCanBo,
                TotalRow: payload.TotalRow,
                TableLoading: false
            };
        case actions.PHANQUYEN_GET_LIST_REQUEST_ERROR:
            return {
                ...state,
                DanhSachNhom: [],
                TotalRow: 0,
                TableLoading: false
            };
        default:
            return state;
    }
}