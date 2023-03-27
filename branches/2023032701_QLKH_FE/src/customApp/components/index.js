import GoInput from "./GoInput/index";
import GoSelect from "./GoSelect/index";
import GoSelect4 from "./GoSelect4/index";
import GoEditor from "./GoEditor/editor";
import GoDatePicker from "./GoDatePicker/index";
import GoTreeSelect from "./GoTreeSelect";
import GoRCSelect from "./GoRCSelect/index";
import GoTruncate from "./GoTruncate/index";
import { withAPI } from "./withAPI";
import { withInputToText } from "./withInputToText";
import PropTypes from "prop-types";

const SelectWithApi = withAPI(GoSelect);
const RCSelectWithApi = withAPI(GoRCSelect);
RCSelectWithApi.propTypes = {
  noInitData: PropTypes.bool,
};
const TreeSelectWithApi = withAPI(GoTreeSelect);

export { GoInput, GoSelect, GoDatePicker, GoTreeSelect, withAPI, TreeSelectWithApi, SelectWithApi, GoEditor, withInputToText, GoRCSelect, RCSelectWithApi, GoSelect4, GoTruncate };
