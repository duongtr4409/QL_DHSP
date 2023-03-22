import { getDefaultPath } from "../../helpers/urlSync";
import actions, { getView } from "./actions";

const preKeys = getDefaultPath();

const initState = {
  collapsed: false,
  view: getView(window.innerWidth),
  height: window.innerHeight,
  openDrawer: false,
  openKeys: preKeys,
  current: preKeys,
  notifications: [],
  // DanhSachHuongDan: [],
  TotalRow: 0,
};

export default function appReducer(state = initState, action) {
  switch (action.type) {
    case actions.COLLPSE_CHANGE:
      return {
        ...state,
        collapsed: !state.collapsed,
      };
    case actions.COLLPSE_OPEN_DRAWER:
      return {
        ...state,
        openDrawer: !state.openDrawer,
      };
    case actions.TOGGLE_ALL:
      if (state.view !== action.view || action.height !== state.height) {
        const height = action.height ? action.height : state.height;
        return {
          ...state,
          collapsed: action.collapsed,
          view: action.view,
          height,
        };
      }
      break;
    case actions.CHANGE_OPEN_KEYS:
      return {
        ...state,
        openKeys: action.openKeys,
      };
    case actions.GET_CONFIG_SUCCESS:
      return {
        ...state,
        systemConfig: action.data,
      };
    case actions.CHANGE_CURRENT:
      return {
        ...state,
        current: action.current,
      };
    case actions.CLEAR_MENU:
      return {
        ...state,
        openKeys: [],
        current: [],
      };
    case "GET_NOTIFICATION_SUCCESS":
      return {
        ...state,
        notifications: action.notifications,
        // DanhSachHuongDan: action.DanhSachHuongDan,
        TotalRow: action.TotalRow,
      };
    default:
      return state;
  }
  return state;
}
