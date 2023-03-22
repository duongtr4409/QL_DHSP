import styled from 'styled-components';

import WithDirection from '../../settings/withDirection';

const WDComponentDivFilter = styled.div`
  padding-bottom: 20px; 
  .ant-select-search, .ant-select, .ant-input-search {
    margin-left: 10px;
    
    &:first-child {
      margin-left: 0;
    }
  }
`;

const ComponentDivFilter = WithDirection(WDComponentDivFilter);
export {ComponentDivFilter};