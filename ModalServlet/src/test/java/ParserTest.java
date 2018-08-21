import com.powerdms.Constants;
import com.powerdms.Parser;
import jdk.nashorn.internal.parser.JSONParser;
import org.hamcrest.core.IsEqual;
import org.junit.Test;
import org.junit.Assert;

import javax.json.Json;
import java.io.IOException;
import java.util.Random;

public class ParserTest {
    private final Parser parser = new Parser();

    @Test
    public void parseQuery_noArguments() throws IOException {
        queryTest("", new String[0]);
    }

    @Test
    public void parseQuery_upload() throws IOException {
        final String query = Test_Constants.CORRECT_QUERY_UPLOAD;
        queryTest(query, Test_Constants.CORRECT_ARGS_UPLOAD);
    }

    @Test
    public void parseQuery_cleanup() throws IOException {
        final String query = Test_Constants.CORRECT_QUERY_CLEANUP;
        queryTest(query, Test_Constants.CORRECT_ARGS_CLEANUP);
    }

    @Test
    public void parseQuery_auth() throws IOException {
        final String query = Test_Constants.CORRECT_QUERY_AUTH;
        queryTest(query, Test_Constants.CORRECT_ARGS_AUTH);
    }

    private void queryTest(String query, String[] expectedArgs){
        final String[] args = parser.parseQuery(query);
        Assert.assertThat(args, IsEqual.equalTo(expectedArgs));
    }

    @Test
    public void constructJson_success() throws Exception {
        final String testOut = "Test output";
        final boolean result = new Random().nextBoolean();

        final String quotedOut = "\"" + testOut + "\"";
        final String quotedEscapedOut = "\\\"" + testOut + "\\\"";

        final String expectedJsonResp = String.format("{\"success\":%b,\"output\":\"%s\"}", result, quotedEscapedOut);

        String jsonResp = parser.requestToJson(result, quotedOut);
        Assert.assertEquals(expectedJsonResp, jsonResp);
    }
}
