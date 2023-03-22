import React from 'react';
import { TreeSelect } from 'antd';

import {getScrollParent} from '../../helpers/utility';

class CustomerTreeSelect extends React.PureComponent{
  render(){
    const props = {
      getPopupContainer: (e) => this.props.getPopupContainer ? this.props.getPopupContainer : getScrollParent(e),
      ...this.props
    };
    return <TreeSelect
      {...props}
      notFoundContent={props.notFoundContent ? props.notFoundContent : "Không có dữ liệu"}
      dropdownStyle={props.dropdownStyle !== undefined ? props.dropdownStyle : { maxHeight: 400, overflow: 'auto' }}
      treeNodeFilterProp={props.treeNodeFilterProp !== undefined ? props.treeNodeFilterProp : "title"}
    />
  }
}

export default CustomerTreeSelect;