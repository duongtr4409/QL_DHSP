import actions from './actions';

const initState = {
    roleQLKH: {view: 0, add: 0, edit: 0, delete: 0},
};

export default function Reducer(state = initState, action){
    const {type, payload} = action;
    switch (type) {
        //get initData
        case actions.DASHBOARD_GET_INIT_DATA_REQUEST:
            return {
                ...state,
                TableLoading: true,
                roleQLKH: payload.roleQLKH,
            };
        case actions.DASHBOARD_GET_INIT_DATA_REQUEST_SUCCESS:
            return {
                ...state,
                TableLoading: false
            };
        case actions.DASHBOARD_GET_INIT_DATA_REQUEST_ERROR:
            return {
                ...state,
                TableLoading: false
            };
        default:
            return state;
    }
}