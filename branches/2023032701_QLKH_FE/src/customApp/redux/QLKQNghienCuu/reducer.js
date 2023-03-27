/*
 *
 * PhanQuyen reducer
 *
 */

import { fromJS } from "immutable";
import * as constants from "./constants";

export const initialState = fromJS({
  DSKQNghienCuu: null,
  loading: false,
  success: null,
});

function QLKQNghienCuuReducer(state = initialState, action) {
  switch (action.type) {
    case constants.GET_KQNC_ACTION:
      return state
        .set("loading", true)

        .set();
    case constants.GET_KQNC_ACTION_SUCCESS:
      return state.setIn(["userData", "repositories"], action.repos).set("loading", false).set("currentUser", action.username);
    case constants.GET_KQNC_ACTION_ERROR:
      return state.set("error", action.error).set("loading", false);
    default:
      return state;
  }
}

export default QLKQNghienCuuReducer;
