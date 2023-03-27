import styled from 'styled-components';
import WithDirection from '../../settings/withDirection';

const WDComponentDivAction = styled.div`
  text-align: right;
  padding: 0 0 5px 0;
  button {
    margin-left: 10px;
  }
`;

const ComponentDivAction = WithDirection(WDComponentDivAction);
export { ComponentDivAction };