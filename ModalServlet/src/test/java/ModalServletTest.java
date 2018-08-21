import com.powerdms.*;
import org.hamcrest.CoreMatchers;
import org.junit.Assert;
import org.junit.Test;
import org.mockito.*;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.io.PrintWriter;

import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.Mockito.*;

public class ModalServletTest {

    @Mock
    private PrintWriter writer;
    @Mock
    private HttpServletRequest req;
    @Mock
    private HttpServletResponse resp;
    @Mock
    private ModalHandler handler;
    @Mock
    private Parser parser;

    public ModalServletTest() throws IOException {
        MockitoAnnotations.initMocks(this);
        when(resp.getWriter()).thenReturn(writer);
        when(req.getQueryString()).thenReturn(Test_Constants.CORRECT_QUERY_UPLOAD);
        when(parser.requestToJson(anyBoolean(), anyString())).thenCallRealMethod();
    }

    @Test
    public void failureCheck_parseQuery() throws IOException {
        when(parser.parseQuery(anyString())).thenReturn(new String[0]);
        new ModalServlet(parser, handler).doGet(req, resp);
        verifyWriter(false, Messages.QUERY_INVALID);
    }

    @Test
    public void failureCheck_loadHandler() throws IOException {
        when(parser.parseQuery(anyString())).thenReturn(Test_Constants.CORRECT_ARGS_UPLOAD);
        when(handler.load()).thenReturn(false);
        new ModalServlet(parser, handler).doGet(req, resp);
        String parsedExpected = Util.aggregate(Test_Constants.CORRECT_ARGS_UPLOAD, Messages.QUERY_PARSED);
        verifyWriter(false, parsedExpected, Messages.HANDLER_LOAD_FAILED);
    }

    @Test
    public void failureCheck_execHandler() throws Exception {
        final int code = 1;
        // Since handling upload dialogs was the primary intent of this program, use it for success check.
        when(parser.parseQuery(anyString())).thenReturn(Test_Constants.CORRECT_ARGS_UPLOAD);
        when(handler.load()).thenReturn(true);
        when(handler.exec(any(String[].class))).thenReturn(code);
        when(handler.getOut()).thenReturn(Test_Constants.HANDLER_OUT);
        String parsedExpected = Util.aggregate(Test_Constants.CORRECT_ARGS_UPLOAD, Messages.QUERY_PARSED);
        new ModalServlet(parser, handler).doGet(req, resp);
        verifyWriter(false, parsedExpected, Messages.HANDLER_LOAD_SUCCESS, Test_Constants.HANDLER_OUT, Messages.HANDLER_EXEC_FAILED_WITH_CODE + code);
    }

    @Test
    public void doGetSuccess() throws Exception {
        when(parser.parseQuery(anyString())).thenCallRealMethod();
        when(handler.load()).thenReturn(true);
        when(handler.exec(any(String[].class))).thenReturn(0);
        when(handler.getOut()).thenReturn(Test_Constants.HANDLER_OUT);
        when(parser.requestToJson(anyBoolean(), anyString())).thenCallRealMethod();
        new ModalServlet(parser, handler).doGet(req, resp);
        verify(handler).exec(Test_Constants.CORRECT_ARGS_UPLOAD);
        String parsedExpected = Util.aggregate(Test_Constants.CORRECT_ARGS_UPLOAD, Messages.QUERY_PARSED);
        verifyWriter(true, parsedExpected, Messages.HANDLER_LOAD_SUCCESS, Test_Constants.HANDLER_OUT, Messages.HANDLER_EXEC_SUCCESS);
    }

    private void verifyWriter(boolean result, Object... lines){
        // Assert that each line is logged omitting timestamp and json formatting.
        verify(writer).write(contains(String.valueOf(result)));
        for (Object line : lines){
            // Conversion to json automatically escapes backslashes.
            // Need to adjust the expectation accordingly.
            String l = line.toString().replace("\\", "\\\\");
            verify(writer).write(contains(l));
        }
    }
}